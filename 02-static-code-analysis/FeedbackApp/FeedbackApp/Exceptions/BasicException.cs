using System.Net;

namespace FeedbackApp.Exceptions;

public class BasicException: HttpBaseException
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
