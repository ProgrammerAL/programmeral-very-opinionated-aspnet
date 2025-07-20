using System.Net;

namespace FeedbackApp.Exceptions;

public abstract class HttpBaseException : Exception
{
    protected HttpBaseException(string message, HttpStatusCode status)
        : base(message)
    {
        HttpStatus = status;
    }

    public HttpStatusCode HttpStatus { get; }
}
