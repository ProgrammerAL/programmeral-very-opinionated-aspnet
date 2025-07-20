using System.ComponentModel.DataAnnotations;

using FeedbackApp.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static FeedbackApp.Endpoints.Ratings.GetRatingsEndpoint.GetRatingsResponse;

namespace FeedbackApp.Endpoints.Ratings;

public static class GetRatingsEndpoint
{
    public static RouteHandlerBuilder RegisterApiEndpoint(RouteGroupBuilder group)
    {
        return group.MapGet("/",
        [AllowAnonymous]
        async ([AsParameters, Required] GetRatingsRequest request,
            IRatingsService ratingsService) =>
            {
                var ratings = await ratingsService.GetRatingsAsync(request.Count);

                var responseItems = ratings.Select(x => new RatingReaponse(x.Rating, x.Comments))
                                           .ToImmutableArray();

                return TypedResults.Ok(new GetRatingsResponse(responseItems));
            })
            .WithSummary("Gets a collection of ratings");
    }

    public class GetRatingsRequest
    {
        [Required, Range(1, 50), FromQuery]
        public required int Count { get; init; }
    }

    public record GetRatingsResponse(ImmutableArray<RatingReaponse> Ratings)
    {
        public record RatingReaponse(int Rating, string Comments);
    }
}
