using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Klippr_Backend.Profile.Infrastructure.Migrations
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
                name: "BusinessProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false),
                    BusinessName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    TaxId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    CategoryValue = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    LocationStreet = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    LocationCity = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    LocationState = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    LocationCountry = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    LocationZipCode = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    LocationLatitude = table.Column<double>(type: "double", nullable: true),
                    LocationLongitude = table.Column<double>(type: "double", nullable: true),
                    Description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    VerificationStatus = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    VerificationDocumentUrl = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    VerificationSubmittedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    VerificationApprovedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RatingAverage = table.Column<double>(type: "double", nullable: true),
                    RatingTotalReviews = table.Column<int>(type: "int", nullable: true),
                    RatingFiveStarCount = table.Column<int>(type: "int", nullable: true),
                    RatingFourStarCount = table.Column<int>(type: "int", nullable: true),
                    RatingThreeStarCount = table.Column<int>(type: "int", nullable: true),
                    RatingTwoStarCount = table.Column<int>(type: "int", nullable: true),
                    RatingOneStarCount = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessProfiles", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ConsumerProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false),
                    FirstName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    LocationStreet = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    LocationCity = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    LocationState = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    LocationCountry = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    LocationZipCode = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    LocationLatitude = table.Column<double>(type: "double", nullable: true),
                    LocationLongitude = table.Column<double>(type: "double", nullable: true),
                    RatingAverage = table.Column<double>(type: "double", nullable: true),
                    RatingTotalReviews = table.Column<int>(type: "int", nullable: true),
                    RatingFiveStarCount = table.Column<int>(type: "int", nullable: true),
                    RatingFourStarCount = table.Column<int>(type: "int", nullable: true),
                    RatingThreeStarCount = table.Column<int>(type: "int", nullable: true),
                    RatingTwoStarCount = table.Column<int>(type: "int", nullable: true),
                    RatingOneStarCount = table.Column<int>(type: "int", nullable: true),
                    SavingsTotalSavings = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SavingsPromotionsUsed = table.Column<int>(type: "int", nullable: true),
                    SavingsPromotionsSaved = table.Column<int>(type: "int", nullable: true),
                    SavingsLastUpdated = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsumerProfiles", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinessProfiles");

            migrationBuilder.DropTable(
                name: "ConsumerProfiles");
        }
    }
}
