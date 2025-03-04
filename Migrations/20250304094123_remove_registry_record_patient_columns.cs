using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VetManagement.Migrations
{
    /// <inheritdoc />
    public partial class remove_registry_record_patient_columns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "RegistryRecords");

            migrationBuilder.DropColumn(
                name: "Identifier",
                table: "RegistryRecords");

            migrationBuilder.DropColumn(
                name: "Sex",
                table: "RegistryRecords");

            migrationBuilder.DropColumn(
                name: "Species",
                table: "RegistryRecords");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Age",
                table: "RegistryRecords",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "Identifier",
                table: "RegistryRecords",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "Sex",
                table: "RegistryRecords",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "Species",
                table: "RegistryRecords",
                type: "longtext",
                nullable: false);
        }
    }
}
