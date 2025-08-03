using Cake.Core;

public record FeedbackAppPaths(
    string ApiProjectDirectory,
    string ApiProjectFilePath,
    string PublishDir,
    string PublishZipFilePath,
    string UnitTestProjectPath,
    string CoverletOutDir)
{
    public static FeedbackAppPaths LoadFromContext(ICakeContext context, string buildConfiguration, string rootDirectory)
    {
        var srcDirectory = $"{rootDirectory}/FeedbackApp";
        var apiProjectDirectory = $"{srcDirectory}/FeedbackApp";
        var apiProjectFilePath = $"{apiProjectDirectory}/FeedbackApp.csproj";
        var publishDir = $"{rootDirectory}/published/FeedbackApp";
        var publishZipFilePath = $"{rootDirectory}/published/feedback-app.zip";
        var unitTestProjFile = $"{srcDirectory}/UnitTests/UnitTests.csproj";
        var coverletOutDir = $"{rootDirectory}/coverlet-coverage-results/";

        return new FeedbackAppPaths(
            apiProjectDirectory,
            apiProjectFilePath,
            publishDir,
            publishZipFilePath,
            unitTestProjFile,
            coverletOutDir);
    }
};
