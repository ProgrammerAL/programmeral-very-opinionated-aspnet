using System.Net;

namespace FeedbackApp.Exceptions;

public abstract class HttpExceptionBase : Exception
{
    protected HttpExceptionBase(string message, HttpStatusCode status)
        : base(message)
    {
        HttpStatus = status;
    }

    public HttpStatusCode HttpStatus { get; }
}
