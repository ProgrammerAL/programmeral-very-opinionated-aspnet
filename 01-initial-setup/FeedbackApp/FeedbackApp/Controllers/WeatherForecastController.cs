using FeedbackApp.Exceptions;

using Microsoft.AspNetCore.Mvc;

using OpenTelemetry.Trace;

namespace FeedbackApp.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly Tracer _tracer;

    public WeatherForecastController(Tracer tracer)
    {
        _tracer = tracer;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        using var span = _tracer.StartActiveSpan("my-get");
        span.SetAttribute("aaa", DateTime.UtcNow.ToString());

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }


    [HttpPost]
    public void Post()
    {
        throw new BasicException("Not implimented yet, hold on!", System.Net.HttpStatusCode.BadRequest);
    }
}
