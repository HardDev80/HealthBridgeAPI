using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthBridgeAPI.Migrations
{
    /// <inheritdoc />
    public partial class LocationClassAddedAndAttributeUploaded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppointmentParticipantLocationId",
                table: "Appointments");

            migrationBuilder.AddColumn<string>(
                name: "PractitionerLocation",
                table: "Practitioners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    LocationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocationName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.LocationId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_LocationId",
                table: "Appointments",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Locations_LocationId",
                table: "Appointments",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Locations_LocationId",
                table: "Appointments");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_LocationId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "PractitionerLocation",
                table: "Practitioners");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Appointments");

            migrationBuilder.AddColumn<string>(
                name: "AppointmentParticipantLocationId",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
