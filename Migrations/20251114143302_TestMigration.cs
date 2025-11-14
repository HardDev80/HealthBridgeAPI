using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthBridgeAPI.Migrations
{
    /// <inheritdoc />
    public partial class TestMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    PatientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientFirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatientLastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatientGender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientMobilePhone = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PatientHomePhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientWorkPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatientBirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PatientDeceased = table.Column<bool>(type: "bit", nullable: false),
                    PatientAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientMaritalStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientPMS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientSSN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatientExtensionRace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientEthnicity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientEmergencyContactPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientComunicationLanguagePreference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientGeneralPractitioner = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientGeneralPractitionerExtensionDateLastSeen = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientReferralResource = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.PatientId);
                });

            migrationBuilder.CreateTable(
                name: "Practitioners",
                columns: table => new
                {
                    PractitionerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PractitionerIdentifier = table.Column<long>(type: "bigint", nullable: false),
                    PractitionerStatus = table.Column<bool>(type: "bit", nullable: false),
                    PractitionerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PractitionerPhotoRef = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PractitionerSpecialty = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PractitionerProvider = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Practitioners", x => x.PractitionerId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserPasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserPasswordSalt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserRole = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserIsActive = table.Column<bool>(type: "bit", nullable: false),
                    UserCreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserLastLoginAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserFailedAttempts = table.Column<int>(type: "int", nullable: false),
                    UserLockoutUntil = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserOrganizationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    AppointmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppointmentStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppointmentCancellationReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppointmentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppointmentReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppointmentDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppointmentComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppointmentStartDateTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppointmentEndDateTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppointmentMinutesDuration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppointmentDateTimeCreated = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppointmentParticipantPatientId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppointmentParticipantPractitionerId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppointmentParticipantLocationId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.AppointmentId);
                    table.ForeignKey(
                        name: "FK_Appointments_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PatientId",
                table: "Appointments",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "Practitioners");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Patients");
        }
    }
}
