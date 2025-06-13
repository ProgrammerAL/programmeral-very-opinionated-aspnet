using FeedbackApp;
using FeedbackApp.Config;
using FeedbackApp.Database;
using FeedbackApp.Endpoints.Ratings;
using FeedbackApp.Middleware;
using FeedbackApp.Services;

using Microsoft.Extensions.Options;

using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddOpenApiServices();

// Add services to the container.

builder.Services.AddSingleton<IRatingsService, RatingsService>();
builder.Services.AddSingleton<IRatingsRepository, RatingsRepository>();

builder.Services.AddOptionsWithValidateOnStart<StorageConfig>().Bind(builder.Configuration.GetSection(nameof(StorageConfig)));
builder.Services.AddSingleton<IValidateOptions<StorageConfig>, StorageConfigValidateOptions>();

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
app.MapRatingsEndpoints();

await app.RunAsync();
