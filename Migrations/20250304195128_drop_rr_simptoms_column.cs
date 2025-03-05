using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VetManagement.Migrations
{
    /// <inheritdoc />
    public partial class drop_rr_simptoms_column : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Symptoms",
                table: "RegistryRecords");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Patients",
                type: "ENUM('pet','livestock')",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "ENUM('pet', 'livestock')");

            migrationBuilder.AlterColumn<string>(
                name: "Sex",
                table: "Patients",
                type: "ENUM('Male','Female')",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "ENUM('Male', 'Female')");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Meds",
                type: "ENUM('medicament','vaccin')",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "ENUM('medicament', 'vaccin')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Symptoms",
                table: "RegistryRecords",
                type: "longtext",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Patients",
                type: "ENUM('pet', 'livestock')",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "ENUM('pet','livestock')");

            migrationBuilder.AlterColumn<string>(
                name: "Sex",
                table: "Patients",
                type: "ENUM('Male', 'Female')",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "ENUM('Male','Female')");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Meds",
                type: "ENUM('medicament', 'vaccin')",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "ENUM('medicament','vaccin')");
        }
    }
}
