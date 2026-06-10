using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Klippr_Backend.Analytics.Infrastructure.Migrations
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
                name: "AbuseReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    ReportedEntityId = table.Column<Guid>(type: "char(36)", nullable: false),
                    ReportedBy = table.Column<Guid>(type: "char(36)", nullable: false),
                    Reason = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<int>(type: "int", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbuseReports", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CampaignMetrics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    CampaignId = table.Column<Guid>(type: "char(36)", nullable: false),
                    BusinessId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Views = table.Column<int>(type: "int", nullable: false),
                    Redemptions = table.Column<int>(type: "int", nullable: false),
                    AverageRating = table.Column<float>(type: "float", nullable: false),
                    RatingCount = table.Column<int>(type: "int", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignMetrics", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AbuseReports_ReportedEntityId",
                table: "AbuseReports",
                column: "ReportedEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_AbuseReports_Status",
                table: "AbuseReports",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignMetrics_BusinessId",
                table: "CampaignMetrics",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignMetrics_CampaignId",
                table: "CampaignMetrics",
                column: "CampaignId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbuseReports");

            migrationBuilder.DropTable(
                name: "CampaignMetrics");
        }
    }
}
