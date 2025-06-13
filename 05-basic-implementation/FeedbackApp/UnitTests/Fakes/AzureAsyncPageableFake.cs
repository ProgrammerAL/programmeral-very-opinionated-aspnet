using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Azure;

using Shouldly;

namespace UnitTests.Fakes;

public class AzureAsyncPageableFake<T> : AsyncPageable<T> where T : notnull
{
    public ImmutableArray<T> FakeItems { get; set; } = [];

    public override async IAsyncEnumerable<Page<T>> AsPages(string? continuationToken = null, int? pageSizeHint = null)
    {
        await Task.CompletedTask;
        var page = new PageFake<T>(FakeItems);
        yield return page;
    }

    private class PageFake<U> : Page<U> where U : notnull
    {
        private readonly ImmutableArray<U> _items;
        public PageFake(ImmutableArray<U> items)
        {
            _items = items;
        }

        public override IReadOnlyList<U> Values => _items;

        public override string? ContinuationToken => throw new NotImplementedException();

        public override Response GetRawResponse()
        {
            throw new NotImplementedException();
        }
    }
}
