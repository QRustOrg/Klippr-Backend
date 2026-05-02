using Klippr_Backend.Promotions.Domain.Aggregates;
using Klippr_Backend.Promotions.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Klippr_Backend.Promotions.Infrastructure.Persistence;

/// <summary>
/// Contexto de EF Core para la persistencia del modulo de promociones.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <remarks>
/// Este contexto queda limitado al bounded context de promociones y no define un proveedor
/// de base de datos concreto.
/// </remarks>
public class PromotionDbContext(DbContextOptions<PromotionDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Conjunto de promociones persistidas.
    /// </summary>
    public DbSet<Promotion> Promotions => Set<Promotion>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Promotion>(promotion =>
        {
            promotion.ToTable("Promotions");

            promotion.HasKey(entity => entity.Id);

            promotion.Property(entity => entity.Id)
                .ValueGeneratedNever();

            promotion.Property(entity => entity.BusinessId)
                .IsRequired();

            promotion.Property(entity => entity.Title)
                .HasMaxLength(120)
                .IsRequired();

            promotion.Property(entity => entity.Description)
                .HasMaxLength(500)
                .IsRequired();

            promotion.Property(entity => entity.RedemptionCap);

            promotion.Property(entity => entity.Status)
                .HasConversion<string>()
                .HasMaxLength(32)
                .IsRequired();

            promotion.Property(entity => entity.CreatedAt)
                .IsRequired();

            promotion.Property(entity => entity.UpdatedAt)
                .IsRequired();

            promotion.OwnsOne<DiscountValue>(entity => entity.Discount, discount =>
            {
                discount.Property(value => value.Amount)
                    .HasColumnName("DiscountAmount")
                    .HasPrecision(18, 2)
                    .IsRequired();

                discount.Property(value => value.Type)
                    .HasColumnName("DiscountType")
                    .HasConversion<string>()
                    .HasMaxLength(32)
                    .IsRequired();
            });

            promotion.OwnsOne<TimeFrame>(entity => entity.ValidityPeriod, validityPeriod =>
            {
                validityPeriod.Property(value => value.StartDate)
                    .HasColumnName("StartDate")
                    .IsRequired();

                validityPeriod.Property(value => value.EndDate)
                    .HasColumnName("EndDate")
                    .IsRequired();

                validityPeriod.HasIndex(value => new { value.StartDate, value.EndDate });
            });

            promotion.Ignore(entity => entity.DomainEvents);

            promotion.HasIndex(entity => entity.BusinessId);
            promotion.HasIndex(entity => entity.Status);
        });
    }
}
