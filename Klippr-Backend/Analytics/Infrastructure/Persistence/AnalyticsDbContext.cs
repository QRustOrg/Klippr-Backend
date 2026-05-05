using Klippr_Backend.Analytics.Domain.Aggregates;
using Klippr_Backend.Analytics.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Klippr_Backend.Analytics.Infrastructure.Persistence;

/// <summary>
/// Contexto de EF Core para la persistencia del módulo de Analytics.
/// </summary>
/// <remarks>
/// Este contexto queda limitado al bounded context de Analytics y no define un proveedor
/// de base de datos concreto.
/// </remarks>
public class AnalyticsDbContext(DbContextOptions<AnalyticsDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Conjunto de métricas de campañas.
    /// </summary>
    public DbSet<CampaignMetrics> CampaignMetrics => Set<CampaignMetrics>();

    /// <summary>
    /// Conjunto de reportes de abuso.
    /// </summary>
    public DbSet<AbuseReport> AbuseReports => Set<AbuseReport>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // CampaignMetrics
        modelBuilder.Entity<CampaignMetrics>(entity =>
        {
            entity.ToTable("CampaignMetrics");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasConversion(
                    id => id.Value,
                    value => new CampaignMetricsId(value)
                )
                .ValueGeneratedNever();

            entity.Property(e => e.CampaignId)
                .IsRequired();

            entity.Property(e => e.BusinessId)
                .IsRequired();

            entity.Property(e => e.Views)
                .IsRequired();

            entity.Property(e => e.Redemptions)
                .IsRequired();

            entity.Property(e => e.AverageRating)
                .IsRequired();

            entity.Property(e => e.LastUpdated)
                .IsRequired();

            entity.HasIndex(e => e.CampaignId);
            entity.HasIndex(e => e.BusinessId);
        });
        
        // AbuseReport
        modelBuilder.Entity<AbuseReport>(entity =>
        {
            entity.ToTable("AbuseReports");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasConversion(
                    id => id.Value,
                    value => new AbuseReportId(value)
                )
                .ValueGeneratedNever();

            entity.Property(e => e.ReportedEntityId)
                .IsRequired();

            entity.Property(e => e.ReportedBy)
                .IsRequired();

            entity.Property(e => e.Reason)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(e => e.Description)
                .HasMaxLength(1000);

            entity.Property(e => e.Status)
                .HasMaxLength(32)
                .IsRequired();

            entity.Property(e => e.CreatedAt)
                .IsRequired();

            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.ReportedEntityId);
        });
    }
}