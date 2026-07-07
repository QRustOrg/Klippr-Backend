using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Klippr_Backend.shared.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddArchivedFavorites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "favorites",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "favorites");
        }
    }
}
