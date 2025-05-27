using System.Net;

namespace FeedbackApp.Exceptions;

public class BasicException: HttpExceptionBase
{
    public BasicException(string message)
        : base(message, HttpStatusCode.InternalServerError)
    {
    }

    public BasicException(string message, HttpStatusCode statusCode)
        : base(message, statusCode)
    {
    }
}
