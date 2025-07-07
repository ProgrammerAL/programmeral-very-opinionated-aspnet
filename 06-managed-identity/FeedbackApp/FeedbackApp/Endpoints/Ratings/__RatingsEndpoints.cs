#pragma warning disable IDE0058 // Expression value is never used

namespace FeedbackApp.Endpoints.Ratings;

public static class RatingsEndpointsEndpoints
{
    public static WebApplication MapRatingsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/ratings");

        var tags = new[] { "Ratings" };

        GetRatingsEndpoint.RegisterApiEndpoint(group).WithTags(tags);
        PostNewRatingEndpoint.RegisterApiEndpoint(group).WithTags(tags);

        return app;
    }
}
#pragma warning restore IDE0058 // Expression value is never used
