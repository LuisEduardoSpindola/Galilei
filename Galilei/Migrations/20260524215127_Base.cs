using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Galilei.Migrations
{
    /// <inheritdoc />
    public partial class Base : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DesiredPrice",
                table: "UserAssets",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "DesiredPriceType",
                table: "UserAssets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsTargetNotified",
                table: "UserAssets",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DesiredPrice",
                table: "UserAssets");

            migrationBuilder.DropColumn(
                name: "DesiredPriceType",
                table: "UserAssets");

            migrationBuilder.DropColumn(
                name: "IsTargetNotified",
                table: "UserAssets");
        }
    }
}
