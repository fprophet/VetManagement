using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VetManagement.Migrations
{
    /// <inheritdoc />
    public partial class create_registry_record_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PatientType",
                table: "Treatments");

            migrationBuilder.AddColumn<string>(
                name: "Administration",
                table: "TreatmentMed",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "WaitingTime",
                table: "TreatmentMed",
                type: "longtext",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Administration",
                table: "TreatmentMed");

            migrationBuilder.DropColumn(
                name: "WaitingTime",
                table: "TreatmentMed");

            migrationBuilder.AddColumn<string>(
                name: "PatientType",
                table: "Treatments",
                type: "longtext",
                nullable: false);
        }
    }
}
