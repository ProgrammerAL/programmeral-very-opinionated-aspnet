using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Azure;

using FeedbackApp.Database;
using FeedbackApp.Database.Entities;
using FeedbackApp.Exceptions;
using FeedbackApp.Services;

using NSubstitute;

using Shouldly;

using UnitTests.Fakes;

namespace UnitTests.FeedbackApp.Services;

public class RatingServiceTests
{
    private readonly IRatingsRepository _ratingsRepo;
    private readonly RatingsService _service;
    public RatingServiceTests()
    {
        _ratingsRepo = Substitute.For<IRatingsRepository>();
        _service = new RatingsService(_ratingsRepo);
    }

    [Fact]
    public async Task WhenStoringNewRating_AssertResult()
    {
        SetStoreRatingResult(status: 200, reasonPhrase: "all-good", content: null);
        await _service.StoreNewRatingAsync(rating: 5, comments: "");
    }

    [Fact]
    public async Task WhenStoringNewRatingErrors_AssertException()
    {
        SetStoreRatingResult(status: 500, reasonPhrase: "bad", content: "unit test error");
        _ = await Should.ThrowAsync<RatingPersistenceException>(async () => await _service.StoreNewRatingAsync(rating: 5, comments: ""));
    }

    [Fact]
    public async Task WhenGettingRatings_AssertResponse()
    {
        var fakeResponse = new AzureAsyncPageableFake<RatingEntity>();
        fakeResponse.FakeItems = 
            [
                new RatingEntity
                { 
                    Rating = 1,
                    Comments = "Hated it"
                },
                new RatingEntity
                { 
                    Rating = 4,
                    Comments = "What I was expecting"
                }
            ];

        _ = _ratingsRepo.GetRatingsAsync(Arg.Any<int>())
                        .Returns(fakeResponse);

        var result = await _service.GetRatingsAsync(count: 3);

        //Requested 3, but only 2 returned is okay
        result.Length.ShouldBe(2);

        result[0].Rating.ShouldBe(1);
        result[0].Comments.ShouldBe("Hated it");

        result[1].Rating.ShouldBe(4);
        result[1].Comments.ShouldBe("What I was expecting");
    }

    private void SetStoreRatingResult(int status, string reasonPhrase, string? content)
    {
        var response = new AzureResponseFake(status, reasonPhrase);

        if (!string.IsNullOrWhiteSpace(content))
        { 
            var contentBytes = Encoding.UTF8.GetBytes(content);
            var memStream = new MemoryStream();
            memStream.Write(contentBytes);

            response.ContentStream = memStream;
        }

        _ = _ratingsRepo.StoreRatingsAsync(Arg.Any<int>(), Arg.Any<string>())
                        .Returns(x => response);
    }
}
