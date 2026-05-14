using Application.OutboundServices;
using Application.QueryServices;
using Application.Services;
using Domain.Repositories;
using Domain.Services;
using Infrastructure.EventPublishing;
using Infrastructure.Persistence;
using Infrastructure.Verification;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddProfileServices(this IServiceCollection services, string connectionString, IConfiguration? configuration = null)
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

        // Register HTTP Client Factory for document storage
        services.AddHttpClient();

        // Register Infrastructure Services with configuration
        var storageBaseUrl = configuration?.GetValue<string>("Storage:BaseUrl") ?? "https://localhost:5001/storage";
        services.AddScoped(provider => 
            new DocumentStorageService(
                provider.GetRequiredService<IHttpClientFactory>(), 
                storageBaseUrl));

        return services;
    }
}
