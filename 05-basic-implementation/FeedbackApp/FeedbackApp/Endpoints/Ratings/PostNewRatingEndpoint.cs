using System.ComponentModel.DataAnnotations;

using FeedbackApp.Services;

using Microsoft.AspNetCore.Mvc;

namespace FeedbackApp.Endpoints.Ratings;

public static class PostNewRatingEndpoint
{
    public static RouteHandlerBuilder RegisterApiEndpoint(RouteGroupBuilder group)
    {
        return group.MapPost("/new-rating",
            async ([FromBody, Required] PostNewRatingRequest requestBody,
            IRatingsService ratingsService) =>
            {
                var comments = requestBody.Body.Comments ?? "";
                await ratingsService.StoreNewRatingAsync(requestBody.Body.Rating, comments);

                return TypedResults.NoContent();
            })
            .WithSummary("Creates and persists a new rating");
    }

    public class PostNewRatingRequest
    {
        [Required, FromBody]
        public required PostNewRatingRequestBody Body { get; set; }

        public class PostNewRatingRequestBody
        {
            [Required, Range(1, 5), FromBody]
            public required int Rating { get; init; }

            [FromBody]
            public string? Comments { get; init; }
        }
    }
}
