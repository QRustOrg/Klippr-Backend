using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Klippr_Backend.IAM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPasswordResetCodeToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "password_reset_code_expires_at",
                table: "Users",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "password_reset_code_hash",
                table: "Users",
                type: "varchar(256)",
                maxLength: 256,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "password_reset_code_expires_at",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "password_reset_code_hash",
                table: "Users");
        }
    }
}
