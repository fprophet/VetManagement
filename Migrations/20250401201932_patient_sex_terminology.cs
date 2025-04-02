using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VetManagement.Migrations
{
    /// <inheritdoc />
    public partial class patient_sex_terminology : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Sex",
                table: "Patients",
                type: "ENUM('Mascul','Femela')",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            //migrationBuilder.AlterColumn<string>(
            //    name: "StreetNumber",
            //    table: "Owners",
            //    type: "varchar(50)",
            //    maxLength: 50,
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "longtext",
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<string>(
            //    name: "County",
            //    table: "Owners",
            //    type: "varchar(100)",
            //    maxLength: 100,
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "longtext",
            //    oldNullable: true);
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
                oldType: "ENUM('Mascul','Femela')");

            //migrationBuilder.AlterColumn<string>(
            //    name: "StreetNumber",
            //    table: "Owners",
            //    type: "longtext",
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "varchar(50)",
            //    oldMaxLength: 50,
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<string>(
            //    name: "County",
            //    table: "Owners",
            //    type: "longtext",
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "varchar(100)",
            //    oldMaxLength: 100,
            //    oldNullable: true);
        }
    }
}
