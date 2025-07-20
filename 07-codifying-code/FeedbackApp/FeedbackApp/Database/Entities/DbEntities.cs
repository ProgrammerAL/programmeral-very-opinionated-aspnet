using System.Diagnostics.CodeAnalysis;

using Azure;
using Azure.Data.Tables;

namespace FeedbackApp.Database.Entities;

public class RatingEntity : ITableEntity
{
    public int? Rating { get; set; }
    public string? Comments { get; set; }

    public string? PartitionKey { get; set; }
    public string? RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }


    [MemberNotNullWhen(true, nameof(Rating), nameof(Comments))]
    public bool IsValid()
        => Rating.HasValue && !string.IsNullOrWhiteSpace(Comments);
}
