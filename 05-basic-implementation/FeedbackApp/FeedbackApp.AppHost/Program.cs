var builder = DistributedApplication.CreateBuilder(args);

var storage = builder.AddAzureStorage("storage").RunAsEmulator();
var tables = storage.AddTables("tables-storage");

builder.AddProject<Projects.FeedbackApp>("feedbackapp")
    .WithUrl("https://localhost:7226/swagger")
    .WithEnvironment("StorageConfig__ConnectionString", tables.Resource.ConnectionStringExpression);

builder.Build().Run();
