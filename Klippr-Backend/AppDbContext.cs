using EntityFrameworkCore.CreatedUpdatedDate.Extensions;
using Klippr_Backend.Favorites.Infrastructure.Persistence;
using Klippr_Backend.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Klippr_Backend.Shared.Infrastructure.Persistence.EFC.Configuration;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.AddCreatedUpdatedInterceptor();
        base.OnConfiguring(builder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Favorites Bounded Context
        builder.ApplyFavoritesConfiguration();

        // Snake_case naming for all tables and columns
        builder.UseSnakeCaseNamingConvention();
    }
}