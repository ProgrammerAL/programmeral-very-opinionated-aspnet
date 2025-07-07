using System.Net;
using System.Text;
using System.Text.Json;

using FeedbackApp.Exceptions;

using OpenTelemetry.Trace;

namespace FeedbackApp.Middleware;

public class ExceptionCatchMiddleware
{
    public record ErrorResponse(string ErrorMessage, string TraceId);

    private readonly RequestDelegate _next;

    public ExceptionCatchMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _ = Tracer.CurrentSpan.RecordException(ex).SetAttribute("error", true);
            Tracer.CurrentSpan.SetStatus(Status.Error);

            HttpStatusCode errorStatus = HttpStatusCode.InternalServerError;
            if (ex is HttpBaseException httpException)
            {
                errorStatus = httpException.HttpStatus;
            }

            var outputException = ex;
            var outputMessageBuilder = new StringBuilder();

            while (outputException != null)
            {
                _ = outputMessageBuilder.Append(outputException.Message + Environment.NewLine + Environment.NewLine);
                outputException = outputException.InnerException;
            }

            var errorObj = new ErrorResponse(ErrorMessage: outputMessageBuilder.ToString(), TraceId: Tracer.CurrentSpan.Context.TraceId.ToString());

            context.Response.StatusCode = (int)errorStatus;
            context.Response.ContentType = "application/json";
            await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(errorObj)));
        }
    }
}
