using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using Rocket.Surgery.Extensions.Testing.Docker;

namespace Rocket.Surgery.Extensions.Marten.Tests
{
    public static class EnsureContainerIsRunningExtensions
    {
        public static Task<string> EnsureContainerIsRunning(
            DockerClient client,
            IEnsureContainerIsRunningContext context,
            CancellationToken token
        ) => EnsureContainerIsRunningInternal(client, context, token);

        public static Task<string> EnsureContainerIsRunning(
            DockerClient client,
            Func<IList<ContainerListResponse>, ContainerListResponse> getContainer,
            Func<CreateContainerParameters, CreateContainerParameters> createContainer,
            Func<ContainerStartParameters, ContainerStartParameters> startContainer,
            Func<ImagesCreateParameters, ImagesCreateParameters> imageCreate,
            CancellationToken token
        ) => EnsureContainerIsRunningInternal(
            client,
            new EnsureContainerIsRunningContext(
                getContainer,
                createContainer,
                startContainer,
                imageCreate
            ),
            token
        );

        private static async Task<string> EnsureContainerIsRunningInternal(
            DockerClient client,
            IEnsureContainerIsRunningContext context,
            CancellationToken token
        )
        {
            string? id;

            async Task<bool> IsContainerRunning(string identity)
            {
                var d = await client.Containers.InspectContainerAsync(identity, token);
                return d.State.Running;
            }

            async Task<ContainerListResponse> GetContainer()
            {
                var containers = await client.Containers.ListContainersAsync(
                    new ContainersListParameters
                    {
                        All = true
                    },
                    token
                );
                return context.GetContainer(containers);
            }

            var listContainer = await GetContainer();
            if (listContainer != null && await IsContainerRunning(listContainer.ID))
            {
                return listContainer.ID;
            }

            if (listContainer != null)
            {
                id = listContainer.ID;
            }
            else
            {
                do
                {
                    if (listContainer == null)
                    {
                        try
                        {
                            var createImageParams = context.CreateImage(new ImagesCreateParameters());
                            await client.Images.CreateImageAsync(
                                createImageParams,
                                new AuthConfig
                                {
                                    ServerAddress = "hub.docker.com"
                                },
                                new Progress<JSONMessage>(),
                                token
                            );

                            var createParams = context.CreateContainer(new CreateContainerParameters());
                            var container = await client.Containers.CreateContainerAsync(createParams, token);
                            id = container.ID;
                        }
                        catch
                        {
                            listContainer = await GetContainer();
                        }
                    }

                    id = listContainer?.ID!;
                    token.ThrowIfCancellationRequested();
                } while (string.IsNullOrEmpty(id));
            }

            if (!await IsContainerRunning(id))
            {
                await client.Containers.StartContainerAsync(
                    id,
                    context.StartContainer(new ContainerStartParameters()),
                    token
                );
            }

            return id;
        }
    }
}