using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Klippr_Backend.Redemption.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "redemptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ConsumerId = table.Column<Guid>(type: "char(36)", nullable: false),
                    BusinessId = table.Column<Guid>(type: "char(36)", nullable: false),
                    PromotionId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Code = table.Column<Guid>(type: "char(36)", nullable: false),
                    UniqueToken = table.Column<Guid>(type: "char(36)", nullable: false),
                    Status = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false),
                    GeneratedAt = table.Column<long>(type: "bigint", nullable: false),
                    RedeemedAt = table.Column<long>(type: "bigint", nullable: true),
                    BlockedAt = table.Column<long>(type: "bigint", nullable: true),
                    ExpiresAt = table.Column<long>(type: "bigint", nullable: false),
                    ValidationMethod = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false),
                    DiscountAppliedAmount = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_redemptions", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_redemptions_BusinessId",
                table: "redemptions",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_redemptions_Code",
                table: "redemptions",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_redemptions_ConsumerId",
                table: "redemptions",
                column: "ConsumerId");

            migrationBuilder.CreateIndex(
                name: "IX_redemptions_PromotionId",
                table: "redemptions",
                column: "PromotionId");

            migrationBuilder.CreateIndex(
                name: "IX_redemptions_PromotionId_ConsumerId",
                table: "redemptions",
                columns: new[] { "PromotionId", "ConsumerId" });

            migrationBuilder.CreateIndex(
                name: "IX_redemptions_Status",
                table: "redemptions",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_redemptions_UniqueToken",
                table: "redemptions",
                column: "UniqueToken",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "redemptions");
        }
    }
}
