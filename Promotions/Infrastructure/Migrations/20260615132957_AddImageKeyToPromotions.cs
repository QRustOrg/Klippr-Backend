using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Klippr_Backend.Promotions.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddImageKeyToPromotions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageKey",
                table: "Promotions",
                type: "varchar(80)",
                maxLength: 80,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageKey",
                table: "Promotions");
        }
    }
}
