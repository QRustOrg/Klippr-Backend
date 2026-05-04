using Application.OutboundServices.Hashing;
using Application.OutboundServices.Tokens;
using Application.Services;
using Domain.Repositories;
using Domain.Services;
using Infrastructure.Hashing;
using Infrastructure.Persistence;
using Infrastructure.Tokens;
using Interface.Facade;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

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
