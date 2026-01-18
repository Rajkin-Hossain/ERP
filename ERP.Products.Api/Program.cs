using ERP.Products.Api.Extensions;
using ERP.Products.Presentation.EndPoints;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddProductServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.UseCors("ApiCors");

// app.UseAuthentication();
// app.UseAuthorization();

app.MapGet("/", () => Results.Redirect("/scalar", permanent: false));

app.MapGet("/health", () => Results.Ok(new { Status = "Healthy" }));

app.MapProductApiEndPoints();

app.Run();


