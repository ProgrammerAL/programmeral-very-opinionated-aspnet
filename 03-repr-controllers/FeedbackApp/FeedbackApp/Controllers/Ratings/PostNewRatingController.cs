using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

namespace FeedbackApp.Controllers.Ratings;

[ApiController]
public class PostNewRatingController : ControllerBase
{
    [HttpPost("/ratings/new-rating")]
    public ActionResult NewRating(PostNewRatingRequest request)
    {
        return NoContent();
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
