using Klippr_Backend.IAM.Application.OutboundServices.Hashing;
using Klippr_Backend.IAM.Application.OutboundServices.Tokens;
using Klippr_Backend.IAM.Application.Services;
using Klippr_Backend.IAM.Domain.Repositories;
using Klippr_Backend.IAM.Domain.Services;
using Klippr_Backend.IAM.Infrastructure.Hashing;
using Klippr_Backend.IAM.Infrastructure.Persistence;
using Klippr_Backend.IAM.Infrastructure.Tokens;
using Klippr_Backend.IAM.Interface.Facade;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Klippr_Backend.IAM.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIamServices(this IServiceCollection services, string connectionString, string jwtSecretKey, int jwtExpirationMinutes = 60)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentException("Connection string cannot be null or empty.", nameof(connectionString));

        if (string.IsNullOrWhiteSpace(jwtSecretKey))
            throw new ArgumentException("JWT secret key cannot be null or empty.", nameof(jwtSecretKey));

        // Database
        services.AddDbContext<IamDbContext>(options =>
            options.UseSqlite(connectionString));

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();

        // Application Services
        services.AddScoped<IUserCommandService, UserCommandService>();
        services.AddScoped<IUserQueryService, UserQueryService>();

        // Infrastructure Services
        services.AddSingleton<IHashingService, HashingService>();
        services.AddSingleton<ITokenService>(new TokenService(jwtSecretKey, jwtExpirationMinutes));

        // Facade
        services.AddScoped<IamContextFacade>();

        return services;
    }

    public static void ApplyIamMigrations(this IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<IamDbContext>();
            context.Database.Migrate();
        }
    }
}
