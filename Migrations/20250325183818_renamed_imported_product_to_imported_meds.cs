using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VetManagement.Migrations
{
    /// <inheritdoc />
    public partial class renamed_imported_product_to_imported_meds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TreatmentImportedMed_ImportedProducts_ImportedMedId",
                table: "TreatmentImportedMed");

            migrationBuilder.DropTable(
                name: "ImportedProducts");

            migrationBuilder.CreateTable(
                name: "ImportedMeds",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false),
                    Cod = table.Column<string>(type: "longtext", nullable: true),
                    CodOnline = table.Column<string>(type: "longtext", nullable: true),
                    CodIntern = table.Column<string>(type: "longtext", nullable: true),
                    Denumire = table.Column<string>(type: "longtext", nullable: true),
                    DenumireScurta = table.Column<string>(type: "longtext", nullable: true),
                    Producator = table.Column<string>(type: "longtext", nullable: true),
                    tva = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    tip = table.Column<string>(type: "longtext", nullable: true),
                    DataInceput = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataSfarsit = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    codBareProducator = table.Column<string>(type: "longtext", nullable: true),
                    um = table.Column<string>(type: "longtext", nullable: true),
                    cantMinima = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PretAmanunt = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Categorie = table.Column<string>(type: "longtext", nullable: true),
                    Link = table.Column<string>(type: "longtext", nullable: true),
                    Sursa = table.Column<string>(type: "longtext", nullable: true),
                    InStocFurnizor = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    EsteElaborabil = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    fractieIntreg = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TipImpachetare = table.Column<string>(type: "longtext", nullable: true),
                    UltimulPretDeIntrare = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UltimulFurnizor = table.Column<string>(type: "longtext", nullable: true),
                    PenUltimulPretDeIntrare = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PenUltimulFurnizor = table.Column<string>(type: "longtext", nullable: true),
                    Fractionabil = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    EsteVizibilLaVanzare = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    VizibilOnline = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    VizibilComandaAndroid = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    PretEuro = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportedMeds", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_TreatmentImportedMed_ImportedMeds_ImportedMedId",
                table: "TreatmentImportedMed",
                column: "ImportedMedId",
                principalTable: "ImportedMeds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TreatmentImportedMed_ImportedMeds_ImportedMedId",
                table: "TreatmentImportedMed");

            migrationBuilder.DropTable(
                name: "ImportedMeds");

            migrationBuilder.CreateTable(
                name: "ImportedProducts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false),
                    Categorie = table.Column<string>(type: "longtext", nullable: true),
                    Cod = table.Column<string>(type: "longtext", nullable: true),
                    CodIntern = table.Column<string>(type: "longtext", nullable: true),
                    CodOnline = table.Column<string>(type: "longtext", nullable: true),
                    DataInceput = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataSfarsit = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Denumire = table.Column<string>(type: "longtext", nullable: true),
                    DenumireScurta = table.Column<string>(type: "longtext", nullable: true),
                    EsteElaborabil = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    EsteVizibilLaVanzare = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    Fractionabil = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    InStocFurnizor = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    Link = table.Column<string>(type: "longtext", nullable: true),
                    PenUltimulFurnizor = table.Column<string>(type: "longtext", nullable: true),
                    PenUltimulPretDeIntrare = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PretAmanunt = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PretEuro = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Producator = table.Column<string>(type: "longtext", nullable: true),
                    Sursa = table.Column<string>(type: "longtext", nullable: true),
                    TipImpachetare = table.Column<string>(type: "longtext", nullable: true),
                    UltimulFurnizor = table.Column<string>(type: "longtext", nullable: true),
                    UltimulPretDeIntrare = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    VizibilComandaAndroid = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    VizibilOnline = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    cantMinima = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    codBareProducator = table.Column<string>(type: "longtext", nullable: true),
                    fractieIntreg = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    tip = table.Column<string>(type: "longtext", nullable: true),
                    tva = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    um = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportedProducts", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_TreatmentImportedMed_ImportedProducts_ImportedMedId",
                table: "TreatmentImportedMed",
                column: "ImportedMedId",
                principalTable: "ImportedProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
