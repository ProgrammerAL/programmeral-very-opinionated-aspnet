using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

using static FeedbackApp.Controllers.Ratings.GetRatingsController.GetRatingsResponse;

namespace FeedbackApp.Controllers.Ratings;

[ApiController]
public class GetRatingsController : ControllerBase
{
    [HttpGet("/ratings")]
    public ActionResult<GetRatingsResponse> GetRatings(GetRatingsRequest request)
    {
        var fakeResponse = new GetRatingsResponse(
            [
                new RatingReaponse(3, "It was okay"),
                new RatingReaponse(5, "The best. I totally wasn't paid to say that"),
                new RatingReaponse(1, "I thought this was supposed to be something else"),
            ]);

        return Ok(fakeResponse);
    }

    public class GetRatingsRequest
    {
        [Required, Range(1, 50), FromQuery]
        public required int Count { get; init; }
    }

    public record GetRatingsResponse(ImmutableArray<RatingReaponse> Ratings)
    {
        public record RatingReaponse(int Rating, string? Comments);
    }
}
