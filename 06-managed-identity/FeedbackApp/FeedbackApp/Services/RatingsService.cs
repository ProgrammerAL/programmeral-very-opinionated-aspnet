using FeedbackApp.Database;
using FeedbackApp.Exceptions;

using static FeedbackApp.Services.RatingsService;

namespace FeedbackApp.Services;

public interface IRatingsService
{
    ValueTask StoreNewRatingAsync(int rating, string comments);
    ValueTask<ImmutableArray<QueriedRating>> GetRatingsAsync(int count);
}

public class RatingsService : IRatingsService
{
    public record QueriedRating(int Rating, string Comments);

    private readonly IRatingsRepository _ratingsRepo;
    public RatingsService(IRatingsRepository ratingsRepo)
    {
        _ratingsRepo = ratingsRepo;
    }

    public async ValueTask StoreNewRatingAsync(int rating, string comments)
    {
        var response = await _ratingsRepo.StoreRatingsAsync(rating, comments);

        if (response.IsError)
        {
            var responseContent = response.Content.ToString();
            throw new RatingPersistenceException($"Error storing new rating. {responseContent}");
        }
    }

    public async ValueTask<ImmutableArray<QueriedRating>> GetRatingsAsync(int count)
    {
        var items = await _ratingsRepo.GetRatingsAsync(count);
        var builder = ImmutableArray.CreateBuilder<QueriedRating>();

        await foreach (var item in items)
        {
            if (item?.IsValid() is true)
            {
                builder.Add(new QueriedRating(item.Rating.Value, item.Comments));
            }
        }

        return builder.ToImmutableArray();
    }
}
