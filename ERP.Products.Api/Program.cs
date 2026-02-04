using ERP.InMemorySagaOrchestrator.Extensions;
using ERP.Products.Api.Extensions;
using ERP.Products.JobSchedule.Hangfire.Extensions;
using ERP.Products.Messaging.InMemory.Extensions;
using ERP.Products.Persistance.PgSQL.Extensions;
using FastEndpoints;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder(args);

//Common for all modules/bounded contexts
builder.Host.AddHostServices();

// Add Product Module application services
builder.Services.AddProductApplicationServices();

// Add Product Module infrastructures
builder.Services.AddHangfireInfrastructure();
builder.Services.AddPgInfrastructure(builder.Configuration);
builder.Services.AddMessagingInfrastructure();

// Add Saga Orchestrator services
builder.Services.AddSagaOrchestratorServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Add migrations for bounded contexts
    await app.Services.MigrateDbContext();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseCors("ApiCors");

app.UseFastEndpoints();

app.UseSwaggerGen();

// Docs shortcut
app.MapGet("/", () => Results.Redirect("/swagger", permanent: false));

app.MapGet("/health", () => Results.Ok(new { Status = "Healthy" }));

app.Run();


