using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VetManagement.Migrations
{
    /// <inheritdoc />
    public partial class treatment_importedmed_mtm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TreatmentImportedMed",
                columns: table => new
                {
                    ImportedMedId = table.Column<string>(type: "varchar(255)", nullable: false),
                    TreatmentId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Pieces = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Administration = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreatmentImportedMed", x => new { x.TreatmentId, x.ImportedMedId });
                    table.ForeignKey(
                        name: "FK_TreatmentImportedMed_ImportedProducts_ImportedMedId",
                        column: x => x.ImportedMedId,
                        principalTable: "ImportedProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TreatmentImportedMed_Treatments_TreatmentId",
                        column: x => x.TreatmentId,
                        principalTable: "Treatments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentImportedMed_ImportedMedId",
                table: "TreatmentImportedMed",
                column: "ImportedMedId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TreatmentImportedMed");
        }
    }
}
