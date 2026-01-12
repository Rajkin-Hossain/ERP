using Amazon;
using Amazon.CognitoIdentityProvider;
using ERP.Auth.Application.Interfaces;
using ERP.Auth.Infrastructure.Identity;
using ERP.Auth.Infrastructure.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace ERP.Auth.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices
        (this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IAmazonCognitoIdentityProvider>(_ =>
        {
            var region = configuration["AWS:Region"]!;
            return new AmazonCognitoIdentityProviderClient(RegionEndpoint.GetBySystemName(region));
        });

        services.Configure<CognitoOptions>(configuration.GetSection("Cognito"));
        services.AddSingleton<IAuthIdentity, AuthCognitoIdentity>();

        // ---------- JWT validation (optional but recommended if you also protect APIs) ----------
        var regionCfg = configuration["AWS:Region"]!;
        var poolIdCfg = configuration["Cognito:UserPoolId"]!;
        var authority = $"https://cognito-idp.{regionCfg}.amazonaws.com/{poolIdCfg}";

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.Authority = authority;
                opt.RequireHttpsMetadata = true;

                // Access tokens can be tricky with audience, so keep it lenient here
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = authority,
                    ValidateLifetime = true,
                    ValidateAudience = false
                };
            });

        services.AddAuthorization();

        return services;
    }
}
