using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VetManagement.Migrations
{
    /// <inheritdoc />
    public partial class tmh_no_reference_to_meds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TreatmentMedHistory_Meds_MedId",
                table: "TreatmentMedHistory");

            migrationBuilder.DropIndex(
                name: "IX_TreatmentMedHistory_MedId",
                table: "TreatmentMedHistory");

            migrationBuilder.DropColumn(
                name: "MedId",
                table: "TreatmentMedHistory");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MedId",
                table: "TreatmentMedHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentMedHistory_MedId",
                table: "TreatmentMedHistory",
                column: "MedId");

            migrationBuilder.AddForeignKey(
                name: "FK_TreatmentMedHistory_Meds_MedId",
                table: "TreatmentMedHistory",
                column: "MedId",
                principalTable: "Meds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
