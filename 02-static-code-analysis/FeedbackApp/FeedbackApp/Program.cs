using FeedbackApp;
using FeedbackApp.Middleware;

using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddOpenApiServices();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddSingleton(x =>
{
    var tracerProvider = x.GetRequiredService<TracerProvider>();
    return tracerProvider.GetTracer(ActivitySourcesInfo.AppActivitySource.Name);
});

var app = builder.Build();

app.MapDefaultEndpoints();
app.AddOpenApiUi();

app.UseMiddleware<ExceptionCatchMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
