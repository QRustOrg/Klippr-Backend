using Klippr_Backend.Redemption.Infrastructure.Persistence.Configuration.Extensions;
using Microsoft.EntityFrameworkCore;
using RedemptionAggregate = Klippr_Backend.Redemption.Domain.Aggregates.Redemption;

namespace Klippr_Backend.Redemption.Infrastructure.Persistence;

/// <summary>
/// Contexto de EF Core para la persistencia del bounded context de canjes.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <remarks>
/// Este contexto no define un proveedor de base de datos concreto ni configura la conexion.
/// </remarks>
public class RedemptionDbContext(DbContextOptions<RedemptionDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Conjunto de canjes persistidos.
    /// </summary>
    public DbSet<RedemptionAggregate> Redemptions => Set<RedemptionAggregate>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyRedemptionConfiguration();
    }
}
