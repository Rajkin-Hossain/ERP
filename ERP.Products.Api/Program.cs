using ERP.Products.Api.Extensions;
using ERP.Products.Persistance.PgSQL.Extensions;
using FastEndpoints;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder(args);

//Common for all modules/bounded contexts
builder.Host.AddHostServices();

// Add Product Module services
builder.Services.AddProductServices(builder.Configuration);

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


