using System.Text;
using ImmersiveView.Identity.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace ImmersiveView.Identity;

public static class IdentityServicesRegistration
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        const string identityUsersConnectionString = "IdentityUsersConnectionString";
        const string configurationIssuerOption = "JwtOptions:Issuer";
        const string configurationAudienceOption = "JwtOptions:Audience";
        const string configurationKeyOption = "JwtOptions:SecretKey";

        services.AddDbContext<ImmersiveViewUsersDbContext>(options => options
            .UseSqlServer(configuration.GetConnectionString(identityUsersConnectionString)));

        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<ImmersiveViewUsersDbContext>()
            .AddDefaultTokenProviders();

        services.AddAuthentication(authenticationOptions =>
        {
            authenticationOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            authenticationOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(jwtBearerOptions =>
        {
            SymmetricSecurityKey securityKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration[configurationKeyOption] ?? string.Empty));

            jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                ValidIssuer = configuration[configurationIssuerOption],
                ValidAudience = configuration[configurationAudienceOption],
                IssuerSigningKey = securityKey
            };
        });

        return services;
    }
}