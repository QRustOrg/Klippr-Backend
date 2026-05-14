using Klippr_Backend.Profile.Application.OutboundServices;
using Klippr_Backend.Profile.Application.QueryServices;
using Klippr_Backend.Profile.Application.Services;
using Klippr_Backend.Profile.Domain.Repositories;
using Klippr_Backend.Profile.Domain.Services;
using Klippr_Backend.Profile.Infrastructure.EventPublishing;
using Klippr_Backend.Profile.Infrastructure.Persistence;
using Klippr_Backend.Profile.Infrastructure.Verification;
using Klippr_Backend.Profile.Interface.Facade;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Klippr_Backend.Profile.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddProfileServices(this IServiceCollection services, string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentException("Connection string cannot be empty.", nameof(connectionString));

        // Register DbContext
        services.AddDbContext<ProfileDbContext>(options =>
            options.UseSqlite(connectionString));

        // Register Repositories
        services.AddScoped<IConsumerProfileRepository, ConsumerProfileRepository>();
        services.AddScoped<IBusinessProfileRepository, BusinessProfileRepository>();

        // Register Domain Services
        services.AddScoped<IConsumerProfileCommandService, ConsumerProfileCommandService>();
        services.AddScoped<IBusinessProfileCommandService, BusinessProfileCommandService>();
        services.AddScoped<IProfileQueryService, ProfileQueryService>();

        // Register Outbound Services
        services.AddScoped<IVerificationService, VerificationService>();
        services.AddScoped<IEventPublisher, ProfileEventPublisher>();
        services.AddScoped<IRatingAggregator, RatingAggregatorService>();

        // Register Infrastructure Services
        services.AddScoped<DocumentStorageService>();

        // Register Facade
        services.AddScoped<ProfileContextFacade>();

        // Register HTTP Client Factory for document storage
        services.AddHttpClient<DocumentStorageService>();

        return services;
    }
}
