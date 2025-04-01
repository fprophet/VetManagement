using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VetManagement.Migrations
{
    /// <inheritdoc />
    public partial class treatment_imp_med_qautnity_as_String : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pieces",
                table: "TreatmentImportedMed");

            migrationBuilder.AlterColumn<string>(
                name: "Quantity",
                table: "TreatmentImportedMed",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "TreatmentImportedMed",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AddColumn<decimal>(
                name: "Pieces",
                table: "TreatmentImportedMed",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
