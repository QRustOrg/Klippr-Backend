using Klippr_Backend.IAM.Domain.Aggregates;
using Klippr_Backend.IAM.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Klippr_Backend.IAM.Infrastructure.Persistence;

public class IamDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;

    public IamDbContext(DbContextOptions<IamDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            entity.Property(e => e.Email)
                .HasConversion(
                    e => e.Value,
                    value => Email.Create(value))
                .HasColumnName("email")
                .IsRequired();

            entity.Property(e => e.PasswordHash)
                .HasColumnName("password_hash")
                .IsRequired();

            entity.Property(e => e.Role)
                .HasConversion(
                    r => r.Value,
                    value => Role.Create(value))
                .HasColumnName("role")
                .IsRequired();

            entity.Property(e => e.FirstName)
                .HasColumnName("first_name")
                .IsRequired();

            entity.Property(e => e.LastName)
                .HasColumnName("last_name")
                .IsRequired(false);

            entity.Property(e => e.BusinessName)
                .HasColumnName("business_name")
                .IsRequired(false);

            entity.Property(e => e.TaxId)
                .HasColumnName("tax_id")
                .IsRequired(false);

            entity.Property(e => e.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true);

            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.Property(e => e.UpdatedAt)
                .HasColumnName("updated_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasIndex(e => e.Email)
                .IsUnique();
        });
    }
}
