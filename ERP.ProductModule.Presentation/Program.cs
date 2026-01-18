using ERP.Products.Presentation.EndPoints;
using ERP.Products.Presentation.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    //await app.Services.MigrateDbContext();

    app.MapOpenApi();
    //scalar api reference
    app.MapScalarApiReference();
}

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseExceptionHandler();

app.UseCors("ApiCors");

//app.UseAuthentication();
//app.UseAuthorization();

app.MapGet("/", () => Results.Redirect("/scalar", permanent: false));

app.MapGet("/health", () => Results.Ok(new { Status = "Healthy" }));

app.MapProductApiEndPoints();

app.Run();