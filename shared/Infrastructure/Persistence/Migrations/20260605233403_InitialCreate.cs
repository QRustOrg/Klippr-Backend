using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Klippr_Backend.shared.Infrastructure.Persistence.Migrations
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
                name: "favorites",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    favorite_id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                    user_id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                    promotion_id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_favorites", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "i_x_favorites_favorite_id",
                table: "favorites",
                column: "favorite_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_favorites_user_id_promotion_id",
                table: "favorites",
                columns: new[] { "user_id", "promotion_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "favorites");
        }
    }
}
