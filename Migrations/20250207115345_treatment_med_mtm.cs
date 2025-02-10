using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VetManagement.Migrations
{
    /// <inheritdoc />
    public partial class treatment_med_mtm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Medication",
                table: "Treatments");

            migrationBuilder.CreateTable(
                name: "TreatmentMed",
                columns: table => new
                {
                    MedId = table.Column<int>(type: "int", nullable: false),
                    TreatmentId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<float>(type: "float", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreatmentMed", x => new { x.TreatmentId, x.MedId });
                    table.ForeignKey(
                        name: "FK_TreatmentMed_Meds_MedId",
                        column: x => x.MedId,
                        principalTable: "Meds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TreatmentMed_Treatments_TreatmentId",
                        column: x => x.TreatmentId,
                        principalTable: "Treatments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentMed_MedId",
                table: "TreatmentMed",
                column: "MedId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TreatmentMed");

            migrationBuilder.AddColumn<string>(
                name: "Medication",
                table: "Treatments",
                type: "mediumtext",
                nullable: false);
        }
    }
}
