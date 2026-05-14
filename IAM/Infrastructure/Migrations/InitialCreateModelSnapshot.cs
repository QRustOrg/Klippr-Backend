using System;
using Klippr_Backend.IAM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Klippr_Backend.IAM.Infrastructure.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(IamDbContext))]
    public partial class InitialCreateModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "10.0.0");

            modelBuilder.Entity("Klippr_Backend.IAM.Domain.Aggregates.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("TEXT");

                    b.Property<string>("BusinessName")
                        .HasColumnName("business_name")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("created_at")
                        .HasColumnType("TEXT")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnName("email")
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnName("first_name")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("is_active")
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(true);

                    b.Property<string>("LastName")
                        .HasColumnName("last_name")
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnName("password_hash")
                        .HasColumnType("TEXT");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnName("role")
                        .HasColumnType("TEXT");

                    b.Property<string>("TaxId")
                        .HasColumnName("tax_id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("updated_at")
                        .HasColumnType("TEXT")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

#pragma warning restore 612, 618
        }
    }
}
