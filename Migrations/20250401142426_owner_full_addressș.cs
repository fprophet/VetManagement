using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VetManagement.Migrations
{
    /// <inheritdoc />
    public partial class owner_full_addressș : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
               name: "Address",
               table: "Owners");

            migrationBuilder.AddColumn<string>(
                name: "Town",
                table: "Owners",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "County",
                table: "Owners",
                type: "varchar(100)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "Owners",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StreetNumber",
                table: "Owners",
                type: "varchar(50)",
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "County",
                table: "Owners");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "Owners");

            migrationBuilder.DropColumn(
                name: "StreetNumber",
                table: "Owners");
            
            migrationBuilder.DropColumn(
                name: "Town",
                table: "Owners");

            migrationBuilder.AddColumn<string>(
                 name: "Address",
                 table: "Owners",
                 type: "longtext",
                 nullable: false);
        }
    }
}
