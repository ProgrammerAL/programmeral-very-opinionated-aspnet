using System.Diagnostics;

namespace FeedbackApp;

public static class ActivitySourcesInfo
{
    public const string ActivitySourcePrefix = "feedback-app";
    public const string ActivitySourcesNameWildcard = $"{ActivitySourcePrefix}.*";
    public static readonly ActivitySource AppActivitySource = new($"{ActivitySourcePrefix}.root");
}
