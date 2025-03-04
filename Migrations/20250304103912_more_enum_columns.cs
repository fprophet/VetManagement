using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VetManagement.Migrations
{
    /// <inheritdoc />
    public partial class more_enum_columns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Sex",
                table: "Patients",
                type: "ENUM('Male', 'Female')",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Meds",
                type: "ENUM('medicament', 'vaccin')",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Sex",
                table: "Patients",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "ENUM('Male', 'Female')");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Meds",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "ENUM('medicament', 'vaccin')");
        }
    }
}
