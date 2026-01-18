using ERP.Products.Api.Extensions;
using ERP.Products.Presentation.EndPoints;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Common API services
builder.Services.AddApiServices(builder.Configuration);

// Add Product Module services
builder.Services.AddProductServices(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseCors("ApiCors");

app.MapGet("/", () => Results.Redirect("/scalar", permanent: false));

app.MapGet("/health", () => Results.Ok(new { Status = "Healthy" }));

app.MapProductApiEndPoints();

app.Run();


