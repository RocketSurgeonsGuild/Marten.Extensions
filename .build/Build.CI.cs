using System.Collections.Generic;
using System.Linq;
using Nuke.Common.CI.GitHubActions;
using Rocket.Surgery.Nuke;
using Rocket.Surgery.Nuke.ContinuousIntegration;
using Rocket.Surgery.Nuke.DotNetCore;
using Rocket.Surgery.Nuke.GithubActions;

[GitHubActionsSteps(
    "ci",
    GitHubActionsImage.MacOsLatest,
    GitHubActionsImage.WindowsLatest,
    GitHubActionsImage.UbuntuLatest,
    AutoGenerate = false,
    On = new[] { GitHubActionsTrigger.Push },
    OnPushTags = new[] { "v*" },
    OnPushBranches = new[] { "master", "next" },
    OnPullRequestBranches = new[] { "master", "next" },
    InvokedTargets = new[] { nameof(Default) },
    NonEntryTargets = new[]
    {
        nameof(ICIEnvironment.CIEnvironment),
        nameof(ITriggerCodeCoverageReports.Trigger_Code_Coverage_Reports),
        nameof(ITriggerCodeCoverageReports.Generate_Code_Coverage_Report_Cobertura),
        nameof(IGenerateCodeCoverageBadges.Generate_Code_Coverage_Badges),
        nameof(IGenerateCodeCoverageReport.Generate_Code_Coverage_Report),
        nameof(IGenerateCodeCoverageSummary.Generate_Code_Coverage_Summary),
        nameof(Default)
    },
    ExcludedTargets = new[] { nameof(ICanClean.Clean), nameof(ICanRestoreWithDotNetCore.DotnetToolRestore) },
    Enhancements = new[] { nameof(Middleware) }
)]
[PrintBuildVersion]
[PrintCIEnvironment]
[UploadLogs]
public partial class Solution
{
    public static RocketSurgeonGitHubActionsConfiguration Middleware(RocketSurgeonGitHubActionsConfiguration configuration)
    {
        var buildJob = configuration.Jobs.First(z => z.Name == "Build");
        var checkoutStep = buildJob.Steps.OfType<CheckoutStep>().Single();
        // For fetch all
        checkoutStep.FetchDepth = 0;
        buildJob.Steps.InsertRange(
            buildJob.Steps.IndexOf(checkoutStep) + 1,
            new BaseGitHubActionsStep[]
            {
                new RunStep("Fetch all history for all tags and branches")
                {
                    Run = "git fetch --prune"
                },
                new SetupDotNetStep("Use .NET Core 2.1 SDK")
                {
                    DotNetVersion = "2.1.x"
                },
                new SetupDotNetStep("Use .NET Core 3.1 SDK")
                {
                    DotNetVersion = "3.1.x"
                },
                new SetupDotNetStep("Use .NET Core 5.0 SDK")
                {
                    DotNetVersion = "5.0.x"
                },
                new SetupDotNetStep("Use .NET Core 6.0 SDK")
                {
                    DotNetVersion = "6.0.x"
                },
            }
        );

        buildJob.Steps.Add(
            new UsingStep("Publish Coverage")
            {
                Uses = "codecov/codecov-action@v1",
                With = new Dictionary<string, string>
                {
                    ["name"] = "actions-${{ matrix.os }}",
                }
            }
        );

        buildJob.Steps.Add(
            new UploadArtifactStep("Publish logs")
            {
                Name = "logs",
                Path = "artifacts/logs/",
                If = "always()"
            }
        );

        buildJob.Steps.Add(
            new UploadArtifactStep("Publish coverage data")
            {
                Name = "coverage",
                Path = "coverage/",
                If = "always()"
            }
        );

        buildJob.Steps.Add(
            new UploadArtifactStep("Publish test data")
            {
                Name = "test data",
                Path = "artifacts/test/",
                If = "always()"
            }
        );

        buildJob.Steps.Add(
            new UploadArtifactStep("Publish NuGet Packages")
            {
                Name = "nuget",
                Path = "artifacts/nuget/",
                If = "always()"
            }
        );

        return configuration;
    }
}