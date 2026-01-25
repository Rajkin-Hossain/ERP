using ERP.SagaOrchestrator.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOrchestratorServices();

var app = builder.Build();

app.Run();
