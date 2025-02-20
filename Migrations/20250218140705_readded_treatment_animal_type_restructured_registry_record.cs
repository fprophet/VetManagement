using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VetManagement.Migrations
{
    /// <inheritdoc />
    public partial class readded_treatment_animal_type_restructured_registry_record : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "RegistryRecords");

            migrationBuilder.AddColumn<string>(
                name: "PatientType",
                table: "Treatments",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "TreatmentId",
                table: "RegistryRecords",
                type: "int",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_RegistryRecords_TreatmentId",
                table: "RegistryRecords",
                column: "TreatmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_RegistryRecords_Treatments_TreatmentId",
                table: "RegistryRecords",
                column: "TreatmentId",
                principalTable: "Treatments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "TreatmentMed",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: false)
                .Annotation("SqlServer:Identity", "1, 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegistryRecords_Treatments_TreatmentId",
                table: "RegistryRecords");

            migrationBuilder.DropIndex(
                name: "IX_RegistryRecords_TreatmentId",
                table: "RegistryRecords");

            migrationBuilder.DropColumn(
                name: "PatientType",
                table: "Treatments");

            migrationBuilder.DropColumn(
                name: "TreatmentId",
                table: "RegistryRecords");

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "RegistryRecords",
                type: "int",
                nullable: false);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "TreatmentMed",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: false)
                .OldAnnotation("SqlServer:Identity", "1, 1");
        }
    }
}
