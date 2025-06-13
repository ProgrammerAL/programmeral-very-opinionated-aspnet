using System.Net;

namespace FeedbackApp.Exceptions;

public class RatingPersistenceException : HttpBaseException
{
    public RatingPersistenceException(string message)
        : base(message, HttpStatusCode.InternalServerError)
    {
    }
}
