using EntityFrameworkCore.CreatedUpdatedDate.Extensions;
using Klippr_Backend.Community.Infrastructure.Persistence.EFC.Configuration.Extensions;
using Klippr_Backend.Favorites.Infrastructure.Persistence;
using Klippr_Backend.Setting.Infrastructure.Persistence.EFC.Configuration.Extensions;
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
        builder.ApplyCommunityConfiguration();
        builder.ApplySettingConfiguration();
        builder.ApplyFavoritesConfiguration(); // ← Favorites
        builder.UseSnakeCaseNamingConvention();
    }
}