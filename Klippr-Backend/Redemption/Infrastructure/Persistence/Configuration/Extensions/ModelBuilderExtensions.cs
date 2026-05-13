using Klippr_Backend.Redemption.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RedemptionAggregate = Klippr_Backend.Redemption.Domain.Aggregates.Redemption;

namespace Klippr_Backend.Redemption.Infrastructure.Persistence.Configuration.Extensions;

/// <summary>
/// Extensiones de configuracion Fluent API para el modelo de canjes.
/// </summary>
/// <author>Samuel Bonifacio</author>
public static class ModelBuilderExtensions
{
    /// <summary>
    /// Aplica la configuracion de persistencia del agregado de canjes.
    /// </summary>
    /// <param name="builder">Constructor del modelo de EF Core.</param>
    public static void ApplyRedemptionConfiguration(this ModelBuilder builder)
    {
        var redemptionCodeConverter = new ValueConverter<RedemptionCode, Guid>(
            code => code.Value,
            value => RedemptionCode.From(value)
        );

        var dateTimeOffsetConverter = new ValueConverter<DateTimeOffset, long>(
            value => value.ToUnixTimeMilliseconds(),
            value => DateTimeOffset.FromUnixTimeMilliseconds(value)
        );

        var nullableDateTimeOffsetConverter = new ValueConverter<DateTimeOffset?, long?>(
            value => value.HasValue ? value.Value.ToUnixTimeMilliseconds() : null,
            value => value.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(value.Value) : null
        );

        builder.Entity<RedemptionAggregate>(redemption =>
        {
            redemption.ToTable("redemptions");

            redemption.HasKey(entity => entity.Id);

            redemption.Property(entity => entity.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            redemption.Property(entity => entity.ConsumerId)
                .IsRequired();

            redemption.Property(entity => entity.BusinessId)
                .IsRequired();

            redemption.Property(entity => entity.PromotionId)
                .HasMaxLength(100)
                .IsRequired();

            redemption.Property(entity => entity.Code)
                .HasConversion(redemptionCodeConverter)
                .IsRequired();

            redemption.Property(entity => entity.UniqueToken)
                .IsRequired();

            redemption.Property(entity => entity.Status)
                .HasConversion<string>()
                .HasMaxLength(32)
                .IsRequired();

            redemption.Property(entity => entity.GeneratedAt)
                .HasConversion(dateTimeOffsetConverter)
                .IsRequired();

            redemption.Property(entity => entity.RedeemedAt)
                .HasConversion(nullableDateTimeOffsetConverter);

            redemption.Property(entity => entity.BlockedAt)
                .HasConversion(nullableDateTimeOffsetConverter);

            redemption.Property(entity => entity.ExpiresAt)
                .HasConversion(dateTimeOffsetConverter)
                .IsRequired();

            redemption.Property(entity => entity.ValidationMethod)
                .HasConversion<string>()
                .HasMaxLength(32)
                .IsRequired();

            redemption.Property(entity => entity.DiscountAppliedAmount)
                .HasPrecision(10, 2)
                .IsRequired();

            redemption.HasIndex(entity => entity.Code)
                .IsUnique();

            redemption.HasIndex(entity => entity.UniqueToken)
                .IsUnique();

            redemption.HasIndex(entity => entity.ConsumerId);
            redemption.HasIndex(entity => entity.BusinessId);
            redemption.HasIndex(entity => entity.PromotionId);
            redemption.HasIndex(entity => entity.Status);
            redemption.HasIndex(entity => new { entity.PromotionId, entity.ConsumerId });
        });
    }
}
