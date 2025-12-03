using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthBridgeAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddAttributteOnLineInPractitioner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PractitionerOnLine",
                table: "Practitioners",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PractitionerOnLine",
                table: "Practitioners");
        }
    }
}
