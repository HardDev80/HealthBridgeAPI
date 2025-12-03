using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthBridgeAPI.Migrations
{
    /// <inheritdoc />
    public partial class NewAttributeInPractitionerClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PractitionerLastName",
                table: "Practitioners",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PractitionerLastName",
                table: "Practitioners");
        }
    }
}
