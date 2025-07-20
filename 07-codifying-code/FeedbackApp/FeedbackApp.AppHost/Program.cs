var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.FeedbackApp>("feedbackapp")
    .WithUrl("https://localhost:7226/swagger");

builder.Build().Run();
