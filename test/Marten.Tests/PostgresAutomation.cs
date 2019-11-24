using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using JetBrains.Annotations;
using Npgsql;
using Rocket.Surgery.Extensions.Testing.Docker;

namespace Rocket.Surgery.Extensions.Marten.Tests
{
    public static class GitRemoteAutomation
    {
        public static string RemoteName { get; }

        static GitRemoteAutomation()
        {
            var directory = Directory.GetCurrentDirectory();
            while (!Directory.Exists(Path.Combine(directory, ".git")))
            {
                directory = Path.GetDirectoryName(directory)!;
            }

            try
            {
                var process = Process.Start(
                    new ProcessStartInfo("git.exe")
                    {
                        WorkingDirectory = directory,
                        Arguments = "remote get-url origin",
                        RedirectStandardOutput = true
                    }
                );

                RemoteName = GetRepositoryName(process.StandardOutput.ReadToEnd()).Trim().ToLower();
            }
            catch
            {
                RemoteName = "unknown";
            }
        }

        private static string GetRepositoryName(string str)
        {
            var last = str.Split('/').Last();
            return last.Split('.').First();
        }
    }

    [PublicAPI]
    public static class PortRandomizer
    {
        public static int GetPort(string assemblyName, int startingPortRange, int endingPortRange)
        {
            var code = Math.Abs(
                BitConverter.ToInt32(
                    MD5.Create().ComputeHash(
                        Encoding.Default.GetBytes($"{GitRemoteAutomation.RemoteName}@{assemblyName}")
                    ),
                    0
                )
            );
            var max = (double)code / int.MaxValue;
            var range = endingPortRange - startingPortRange;
            return (int)( ( range * max ) + startingPortRange );
        }
    }

#pragma warning disable CS0436 // Type conflicts with imported type
    [PublicAPI]
#pragma warning restore CS0436 // Type conflicts with imported type
    public class PostgresAutomation : IEnsureContainerIsRunningContext
    {
        public static PostgresAutomation ForLocalDevelopment(TimeSpan timeout = default)
        {
            if (timeout == default)
            {
                timeout = TimeSpan.FromSeconds(30);
            }

            var containerName = "marten-development";

            var connectionStringBuilder = new NpgsqlConnectionStringBuilder
            {
                Host = "localhost",
                Port = 5434,
                Username = "myuser",
                Password = "mypassword",
                Database = "development"
            };
            return new PostgresAutomation(
                connectionStringBuilder.ToString(),
                containerName,
                GitRemoteAutomation.RemoteName,
                timeout
            );
        }

        public static PostgresAutomation ForOther(string containerName, TimeSpan timeout = default)
        {
            if (timeout == default)
            {
                timeout = TimeSpan.FromSeconds(30);
            }

            var connectionStringBuilder = new NpgsqlConnectionStringBuilder
            {
                Host = "localhost",
                Port = PortRandomizer.GetPort(containerName, 40000, 49999),
                Username = "myuser",
                Password = "mypassword",
                Database = "development"
            };
            return new PostgresAutomation(
                connectionStringBuilder.ToString(),
                containerName,
                GitRemoteAutomation.RemoteName,
                timeout
            );
        }

        public static PostgresAutomation ForUnitTesting(Assembly assembly, TimeSpan timeout = default)
        {
            if (timeout == default)
            {
                timeout = TimeSpan.FromMinutes(5);
            }

            var databaseName = "marten" + Guid.NewGuid().ToString("N");
            var containerName = "marten-tests";

            var connectionStringBuilder = new NpgsqlConnectionStringBuilder
            {
                Host = "localhost",
                Port = 65434,
                Username = "myuser",
                Password = "mypassword",
                Database = "unittesting_db"
            };

            return new PostgresAutomation(connectionStringBuilder.ToString(), containerName, databaseName, timeout);
        }

        public static PostgresAutomation ForUnitTesting(Type type, TimeSpan timeout = default)
            => ForUnitTesting(type.Assembly, timeout);

        private static readonly SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(1, 1);
        private readonly string _containerName;
        private readonly string _databaseName;
        private readonly List<string> _logs = new List<string>();
        private readonly string _originalDatabaseName;
        private readonly CancellationToken _token;

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        private PostgresAutomation(
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
            string connectionString,
            string containerName,
            string finalDatabaseName,
            TimeSpan timeout
        )
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("message", nameof(connectionString));
            }

            if (string.IsNullOrWhiteSpace(containerName))
            {
                throw new ArgumentException("message", nameof(containerName));
            }

            if (timeout == default)
            {
                timeout = TimeSpan.FromMinutes(5);
            }

            var cts = new CancellationTokenSource();
            cts.CancelAfter(timeout);
            _token = cts.Token;

            ConnectionString = new NpgsqlConnectionStringBuilder(connectionString);
            _containerName = containerName;
            _databaseName = finalDatabaseName ?? ConnectionString.Database!;
            _originalDatabaseName = ConnectionString.Database!;

            static Uri localDockerUri()
            {
                var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
                return isWindows ? new Uri("npipe://./pipe/docker_engine") : new Uri("unix:/var/run/docker.sock");
            }

            Client = new DockerClientConfiguration(localDockerUri()).CreateClient();
        }

        public NpgsqlConnectionStringBuilder ConnectionString { get; }
        public DockerClient Client { get; }
        public string Id { get; private set; }
        public IEnumerable<string> Logs => _logs;

        public async Task Start()
        {
            await SemaphoreSlim.WaitAsync(_token);

            _logs.Add("Creating database container (if not created)");

            Id = await EnsureContainerIsRunningExtensions.EnsureContainerIsRunning(Client, this, _token);

            _logs.Add("Waiting for the database to be available");

            await WaitForDatabaseToBeAvailable();

            SemaphoreSlim.Release();

            await CreateDatabase();

            ConnectionString.Database = _databaseName;
        }

        private async Task WaitForDatabaseToBeAvailable()
        {
            while (!_token.IsCancellationRequested)
            {
                try
                {
                    using (var c = new NpgsqlConnection(ConnectionString.ToString()))
                    {
                        c.Open();
                        var command = c.CreateCommand();
                        command.CommandText = "SELECT * from pg_catalog.pg_tables;";
                        command.ExecuteNonQuery();
                    }

                    return;
                }
                catch
                {
                    await Task.Delay(50, _token);
                }
            }
        }

        private async Task CreateDatabase()
        {
            using (var c = new NpgsqlConnection(ConnectionString.ToString()))
            {
                c.Open();
                var cc = c.CreateCommand();
                cc.CommandText = $"SELECT EXISTS (SELECT 1 FROM pg_database WHERE datname = '{_databaseName}')";
                var hasDatabase = (bool)await cc.ExecuteScalarAsync(_token);
                if (!hasDatabase)
                {
                    var command = c.CreateCommand();
                    command.CommandText = $"CREATE DATABASE {_databaseName};";
                    await command.ExecuteNonQueryAsync(_token);
                }
            }
        }

        public async Task DropDatabase()
        {
            if (_originalDatabaseName == _databaseName)
            {
                return;
            }

            var connectionString = new NpgsqlConnectionStringBuilder(ConnectionString.ToString())
            {
                Database = _originalDatabaseName
            };
            await using var c = new NpgsqlConnection(connectionString.ToString());
            c.Open();
            var command = c.CreateCommand();
            command.CommandText = $@"
UPDATE pg_database SET datallowconn = 'false' WHERE datname = '{
                    _databaseName
                }';
ALTER DATABASE {
                    _databaseName
                } CONNECTION LIMIT 1;

SELECT pg_terminate_backend(pid) FROM pg_stat_activity WHERE datname = '{
                    _databaseName
                }';

DROP DATABASE {
                    _databaseName
                };";
            await command.ExecuteNonQueryAsync(_token);
        }

        ContainerListResponse IEnsureContainerIsRunningContext.GetContainer(IList<ContainerListResponse> responses)
            => responses.SingleOrDefault(x => x.Names.Any(name => name == $"/{_containerName}"));

        CreateContainerParameters IEnsureContainerIsRunningContext.CreateContainer(
            CreateContainerParameters createContainerParameters
        ) => new CreateContainerParameters
        {
            Name = _containerName,
            Image = "postgres:9.6",
            Env = new List<string>
            {
                $"POSTGRES_USER={ConnectionString.Username}",
                $"POSTGRES_PASSWORD={ConnectionString.Password}",
                $"POSTGRES_DB={ConnectionString.Database}"
            },
            HostConfig = new HostConfig
            {
                PortBindings = new Dictionary<string, IList<PortBinding>>
                {
                    {
                        "5432/tcp",
                        new List<PortBinding> { new PortBinding { HostPort = ConnectionString.Port.ToString() } }
                    }
                }
            }
        };

        ContainerStartParameters IEnsureContainerIsRunningContext.StartContainer(
            ContainerStartParameters containerStartParameters
        ) => containerStartParameters;

        ImagesCreateParameters IEnsureContainerIsRunningContext.CreateImage(
            ImagesCreateParameters imagesCreateParameters
        )
        {
            imagesCreateParameters.FromImage = "postgres";
            imagesCreateParameters.Tag = "9.6";
            return imagesCreateParameters;
        }
    }
}