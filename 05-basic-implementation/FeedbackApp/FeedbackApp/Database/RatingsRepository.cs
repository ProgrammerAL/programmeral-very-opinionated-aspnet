using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Azure.Data.Tables;

using FeedbackApp.Config;
using FeedbackApp.Database.Entities;

using Microsoft.Extensions.Options;

namespace FeedbackApp.Database;

public interface IRatingsRepository
{
    ValueTask<Azure.Response> StoreRatingsAsync(int rating, string comments);
    ValueTask<Azure.AsyncPageable<RatingEntity>> GetRatingsAsync(int count);
}

public class RatingsRepository : IRatingsRepository
{
    private readonly IOptions<StorageConfig> _storageConfig;

    public RatingsRepository(IOptions<StorageConfig> storageConfig)
    {
        _storageConfig = storageConfig;
    }

    public async ValueTask<Azure.Response> StoreRatingsAsync(int rating, string comments)
    {
        var tableClient = await LoadTableClientAsync();

        var itemKey = Guid.NewGuid().ToString();
        var entity = new RatingEntity
        {
            PartitionKey = itemKey,
            RowKey = itemKey,
            Rating = rating,
            Comments = comments,
        };

        return await tableClient.AddEntityAsync(entity);
    }

    public async ValueTask<Azure.AsyncPageable<RatingEntity>> GetRatingsAsync(int count)
    {
        var tableClient = await LoadTableClientAsync();
        var results = tableClient.QueryAsync<RatingEntity>(maxPerPage: count);
        return results;
    }

    private async ValueTask<TableClient> LoadTableClientAsync()
    {
        var tableClient = new TableClient(_storageConfig.Value.ConnectionString, _storageConfig.Value.TableName);
        _ = await tableClient.CreateIfNotExistsAsync();

        return tableClient;
    }
}
