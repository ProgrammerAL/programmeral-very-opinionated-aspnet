using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Azure.Data.Tables;
using Azure.Identity;

using FeedbackApp.AzureUtils;
using FeedbackApp.Config;
using FeedbackApp.Database.Entities;

using Microsoft.Extensions.Options;

using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;

namespace FeedbackApp.Database;

[GenerateInterface]
public class RatingsRepository : IRatingsRepository
{
    private readonly IOptions<StorageConfig> _storageConfig;
    private readonly IAzureCredentialsManager _credsManager;

    public RatingsRepository(IOptions<StorageConfig> storageConfig, IAzureCredentialsManager credsManager)
    {
        _storageConfig = storageConfig;
        _credsManager = credsManager;
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
        var uri = new Uri(_storageConfig.Value.Url);
        var tableClient = new TableClient(
            uri, 
            tableName: _storageConfig.Value.TableName,
            tokenCredential: _credsManager.AzureTokenCredential);
        _ = await tableClient.CreateIfNotExistsAsync();

        return tableClient;
    }
}
