using ERP.Products.Dispatcher.Hangfire.Extensions;
using ERP.Products.Messaging.RabbitMQ.Extensions;
using ERP.Products.Persistance.MongoDb.Extensions;
using ERP.Products.Presentation.Extensions;
using ERP.Shared.Domain.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;

namespace ERP.Products.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAddProblemDetailsServices(configuration);
        services.AddOpenApi();
        services.AddCorsServices(configuration);

        return services;
    }

    public static IServiceCollection AddProductServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPresentationServices(configuration);

        services.AddMongoInfrastructure(configuration);
        services.AddMessageBusInfrastructure();
        services.AddHangfireInfrastructure();
        //services.AddOutboxBackgroundWorker();

        return services;
    }

    private static IServiceCollection AddAddProblemDetailsServices(this IServiceCollection services, IConfiguration configuration)
    {
        //For unhandled exceptions
        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                var exception = context.HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

                if (exception is DomainException domainEx)
                {
                    context.ProblemDetails.Status = StatusCodes.Status400BadRequest;
                    context.ProblemDetails.Title = "Domain Rule Violation";
                    context.ProblemDetails.Detail = domainEx.Message;
                    context.ProblemDetails.Type = "https://erp.com/errors/domain-violation";
                }
                else if (exception != null)
                {
                    // This adds a 'rawError' field to your JSON output
                    context.ProblemDetails.Extensions["rawError"] = exception.Message;
                    context.ProblemDetails.Extensions["stackTrace"] = exception.StackTrace;
                }
            };
        });

        return services;
    }

    private static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration configuration)
    {
        var region = configuration["Auth:Region"]!;
        var userPoolId = configuration["Auth:UserPoolId"]!;
        var clientId = configuration["Auth:ClientId"]!;

        // Cognito issuer (authority)
        var authority = $"https://cognito-idp.{region}.amazonaws.com/{userPoolId}";

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = authority;

                // Most Cognito JWT validation just works with Authority.
                // These validations are the important ones:
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = authority,

                    ValidateLifetime = true,

                    // ? IMPORTANT for Cognito Access Tokens
                    // Access tokens often don't have aud the way you expect.
                    ValidateAudience = false,
                };
            });

        // --- Authorization policies based on Cognito groups ---
        services.AddAuthorizationBuilder()
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

        return services;
    }

    private static IServiceCollection AddCorsServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("ApiCors", policy =>
            {
                var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
                if (allowedOrigins is { Length: > 0 })
                {
                    policy.WithOrigins(allowedOrigins)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                }
                else
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                }
            });
        });

        return services;
    }
}




