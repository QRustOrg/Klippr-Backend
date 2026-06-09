using Klippr_Backend.IAM.Application.OutboundServices.Hashing;
using Klippr_Backend.IAM.Domain.Aggregates;
using Klippr_Backend.IAM.Domain.Repositories;
using Klippr_Backend.IAM.Domain.ValueObjects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Klippr_Backend.IAM.Infrastructure;

public static class IamSeeder
{
    public static async Task SeedAdminAsync(IServiceProvider serviceProvider, IConfiguration configuration, CancellationToken cancellationToken = default)
    {
        using var scope = serviceProvider.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger("IamSeeder");

        var email = configuration["AdminSeed:Email"];
        var password = configuration["AdminSeed:Password"];

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            logger.LogInformation("AdminSeed:Email or AdminSeed:Password not configured. Skipping admin seed.");
            return;
        }

        var firstName = configuration["AdminSeed:FirstName"];
        var lastName = configuration["AdminSeed:LastName"];
        if (string.IsNullOrWhiteSpace(firstName)) firstName = "Admin";
        if (string.IsNullOrWhiteSpace(lastName)) lastName = "Klippr";

        var repository = services.GetRequiredService<IUserRepository>();
        var hashingService = services.GetRequiredService<IHashingService>();

        Email emailValue;
        try
        {
            emailValue = Email.Create(email);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning("AdminSeed:Email is invalid ({Message}). Skipping admin seed.", ex.Message);
            return;
        }

        var exists = await repository.ExistsByEmailAsync(emailValue, cancellationToken);
        if (exists)
        {
            logger.LogInformation("Admin user already exists. Skipping admin seed.");
            return;
        }

        var passwordHash = hashingService.Hash(password);
        var admin = User.CreateAdmin(emailValue, passwordHash, firstName, lastName);
        await repository.AddAsync(admin, cancellationToken);

        logger.LogInformation("Admin user seeded: {Email}", emailValue.Value);
    }
}
