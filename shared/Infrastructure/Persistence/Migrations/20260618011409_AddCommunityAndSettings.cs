using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Klippr_Backend.shared.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCommunityAndSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "community_reviews",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    promotion_id = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    redemption_id = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    rating = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    comment = table.Column<string>(type: "varchar(240)", maxLength: 240, nullable: false),
                    review_id = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    content = table.Column<string>(type: "varchar(240)", maxLength: 240, nullable: false),
                    business_id = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_community_reviews", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "preferences",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    dark_mode = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    language_code = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    timezone = table.Column<string>(type: "longtext", nullable: false),
                    email_notifications = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    push_notifications = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    sms_notifications = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    profile_visibility = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    data_sharing_consent = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_preferences", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "community_reviews");

            migrationBuilder.DropTable(
                name: "preferences");
        }
    }
}
