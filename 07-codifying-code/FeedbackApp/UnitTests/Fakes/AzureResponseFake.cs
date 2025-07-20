using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Azure.Core;

namespace UnitTests.Fakes;

public class AzureResponseFake : Azure.Response
{
    private int _status;
    private string _reasonPhrase = "";

    public AzureResponseFake()
    { 
    }

    public AzureResponseFake(int status, string reasonPhrase)
    { 
        SetReturnValues(status, reasonPhrase);
    }

    public void SetReturnValues(int status, string reasonPhrase)
    {
        _status = status;
        _reasonPhrase = reasonPhrase;
    }

    public override int Status => _status;

    public override string ReasonPhrase => _reasonPhrase;

    public override bool IsError => _status < 200 || _status > 299;

    public override Stream? ContentStream { get; set; }
    public override string ClientRequestId { get; set; } = "";

    public override void Dispose()
    {
    }

    protected override bool ContainsHeader(string name)
    {
        return false;
    }

    protected override IEnumerable<HttpHeader> EnumerateHeaders()
    {
        return [];
    }

    protected override bool TryGetHeader(string name, [NotNullWhen(true)] out string? value)
    {
        value = null;
        return false;
    }

    protected override bool TryGetHeaderValues(string name, [NotNullWhen(true)] out IEnumerable<string>? values)
    {
        values = null;
        return false;
    }
}
