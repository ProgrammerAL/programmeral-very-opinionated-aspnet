using System.IO.Compression;

using Cake.Common;
using Cake.Common.IO;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Build;
using Cake.Common.Tools.DotNet.Publish;
using Cake.Common.Tools.DotNet.Test;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Frosting;

public static class Program
{
    public static int Main(string[] args)
    {
        return new CakeHost()
            .UseContext<BuildContext>()
            .Run(args);
    }
}

public class BuildContext : FrostingContext
{
    public string Target { get; } = "Default";
    public string BuildConfiguration { get; }
    public string RootDirectoryPath { get; }
    public FeedbackAppPaths FeedbackAppPaths { get; }

    public BuildContext(ICakeContext context)
        : base(context)
    {
        BuildConfiguration = LoadParameter(context, nameof(BuildConfiguration));
        RootDirectoryPath = LoadParameter(context, nameof(RootDirectoryPath));

        FeedbackAppPaths = FeedbackAppPaths.LoadFromContext(context, BuildConfiguration, RootDirectoryPath);
    }

    private string LoadParameter(ICakeContext context, string parameterName)
    {
        return context.Arguments.GetArgument(parameterName) ?? throw new Exception($"Missing parameter '{parameterName}'");
    }
}

[TaskName(nameof(OutputParametersTask))]
public sealed class OutputParametersTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.Log.Information($"INFO: Current Working Directory: {context.Environment.WorkingDirectory}");

        context.Log.Information($"INFO: {nameof(context.BuildConfiguration)}: {context.BuildConfiguration}");
        context.Log.Information($"INFO: {nameof(context.RootDirectoryPath)}: {context.RootDirectoryPath}");
        context.Log.Information($"INFO: {nameof(context.FeedbackAppPaths)}: {context.FeedbackAppPaths}");
    }
}

[IsDependentOn(typeof(OutputParametersTask))]
[TaskName(nameof(CleanTask))]
public sealed class CleanTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.CleanDirectory(context.FeedbackAppPaths.PublishDir);
    }
}

[IsDependentOn(typeof(CleanTask))]
[TaskName(nameof(DotNetBuildTask))]
public sealed class DotNetBuildTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        BuildDotnetApp(context, context.FeedbackAppPaths.ApiProjectFilePath);
        BuildDotnetApp(context, context.FeedbackAppPaths.UnitTestProjectPath);

        RunUnitTests(context, context.FeedbackAppPaths.UnitTestProjectPath);
    }

    private void BuildDotnetApp(BuildContext context, string projectFilePath)
    {
        context.DotNetBuild(projectFilePath, new DotNetBuildSettings
        {
            NoRestore = false,
            Configuration = context.BuildConfiguration
        });
    }

    private void RunUnitTests(BuildContext context, string projectPath)
    {
        var testSettings = new DotNetTestSettings()
        {
            Configuration = context.BuildConfiguration,
            NoBuild = true,
            ArgumentCustomization = (args) => args.Append("/p:CollectCoverage=true /p:CoverletOutputFormat=cobertura --logger trx")
        };

        context.DotNetTest(projectPath, testSettings);
    }
}

[IsDependentOn(typeof(DotNetBuildTask))]
[TaskName(nameof(PublishApplicationsTask))]
public sealed class PublishApplicationsTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        PublishFeedbackApi(context.FeedbackAppPaths, context);
    }

    private void PublishFeedbackApi(FeedbackAppPaths paths, BuildContext context)
    {
        DeleteAppSettingsFiles(paths.ApiProjectDirectory, context);
        var settings = new DotNetPublishSettings()
        {
            NoRestore = true,
            NoBuild = true,
            Configuration = context.BuildConfiguration,
            OutputDirectory = paths.PublishDir,
        };

        context.DotNetPublish(paths.ApiProjectFilePath, settings);

        //Now that the code is published, create the compressed folder
        if (!Directory.Exists(paths.PublishDir))
        {
            _ = Directory.CreateDirectory(paths.PublishDir);
        }

        if (File.Exists(paths.PublishZipFilePath))
        {
            File.Delete(paths.PublishZipFilePath);
        }

        ZipFile.CreateFromDirectory(paths.PublishDir, paths.PublishZipFilePath);
        context.Log.Information($"Output Feedback App zip file to: {paths.PublishZipFilePath}");
    }

    private void DeleteAppSettingsFiles(string directoryPath, BuildContext context)
    {
        var appsettingsFilePath = $"{directoryPath}/appsettings.json";

        if (File.Exists(appsettingsFilePath))
        {
            File.Delete(appsettingsFilePath);
        }

        var extraAppSettingsFiles = Directory.GetFiles(directoryPath, "appsettings.*.json");
        foreach (var file in extraAppSettingsFiles)
        {
            context.Log.Information($"Deleting extra appsettings file: {file}");
            File.Delete(file);
        }
    }
}

[IsDependentOn(typeof(PublishApplicationsTask))]
[TaskName("Default")]
public class DefaultTask : FrostingTask
{
}
