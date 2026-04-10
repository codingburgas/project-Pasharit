using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SleepTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddFactorValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "SleepFactors",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "SleepFactors");
        }
    }
}
