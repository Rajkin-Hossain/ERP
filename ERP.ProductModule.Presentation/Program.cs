using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var region = builder.Configuration["Auth:Region"]!;
var userPoolId = builder.Configuration["Auth:UserPoolId"]!;
var clientId = builder.Configuration["Auth:ClientId"]!;

// Cognito issuer (authority)
var authority = $"https://cognito-idp.{region}.amazonaws.com/{userPoolId}";

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = authority;

        // Most Cognito JWT validation “just works” with Authority.
        // These validations are the important ones:
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = authority,

            ValidateLifetime = true,

            // ✅ IMPORTANT for Cognito Access Tokens
            // Access tokens often don't have aud the way you expect.
            ValidateAudience = false,
        };
    });

// --- Authorization policies based on Cognito groups ---
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AdminOnly", p =>
        p.RequireAuthenticatedUser()
            .RequireClaim("cognito:groups", "admin-group"))

    .AddPolicy("CustomerOnly", p =>
        p.RequireAuthenticatedUser()
         .RequireClaim("cognito:groups", "customer-group"))
    
    .AddPolicy("AnyUser", p =>
        p.RequireAuthenticatedUser()
         .RequireAssertion(ctx =>
    {
        var groups = ctx.User.FindAll("cognito:groups").Select(c => c.Value);
        return groups.Contains("admin-group") || groups.Contains("customer-group");
    }
));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/me", (System.Security.Claims.ClaimsPrincipal user) =>
{
    var sub = user.FindFirst("sub")?.Value;
    var username = user.FindFirst("cognito:username")?.Value ?? user.Identity?.Name;
    var groups = user.FindAll("cognito:groups").Select(x => x.Value).ToArray();

    return Results.Ok(new { sub, username, groups });
})
.RequireAuthorization("AnyUser");


app.MapGet("/weatherforecast2", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast2");

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.RequireAuthorization("AdminOnly");

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
