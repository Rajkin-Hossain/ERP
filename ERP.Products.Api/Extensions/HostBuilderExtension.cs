using ERP.Products.CommandHandlers.Commands;
using ERP.Products.CommandHandlers.Pipelines;
using ERP.Shared.Domain.Exceptions;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Wolverine;

namespace ERP.Products.Api.Extensions;

public static class HostBuilderExtension
{
    public static ConfigureHostBuilder AddHostServices(this ConfigureHostBuilder configureHostBuilder)
    {
        configureHostBuilder.UseWolverine(opts =>
        {
            // Discover all referenced assemblies automatically
            var assemblies = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(a => !a.IsDynamic);

            foreach (var asm in assemblies)
                opts.Discovery.IncludeAssembly(asm);

            // Apply pipeline to ALL commands
            opts.Policies.ForMessagesOfType<CreateProductCommand>()
                .AddMiddleware<CreateProductPipeline>();
        });

        configureHostBuilder.ConfigureServices((context, services) =>
        {
            services.AddFastEndpoints();
            services.SwaggerDocument(o =>
            {
                o.DocumentSettings = s =>
                {
                    s.Title = "My API";
                    s.Version = "v1";
                    s.Description = "FastEndpoints + OpenAPI";
                };
                // optional: short schema names (nice for DTOs)
                o.ShortSchemaNames = true;
            });
            services.AddAddProblemDetailsServices(context.Configuration);
            services.AddCorsServices(context.Configuration);
        });

        return configureHostBuilder;
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
        // Always register authN/authZ services so endpoints with authorization metadata don't crash
        // even if an auth provider isn't configured yet.
        var region = configuration["Auth:Region"];
        var userPoolId = configuration["Auth:UserPoolId"];

        if (string.IsNullOrWhiteSpace(region) || string.IsNullOrWhiteSpace(userPoolId))
        {
            services.AddAuthentication();
            services.AddAuthorization();
            return services;
        }

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