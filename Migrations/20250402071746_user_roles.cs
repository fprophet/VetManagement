using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VetManagement.Migrations
{
    /// <inheritdoc />
    public partial class user_roles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "ENUM('manager','farmacist','asistent')",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(10)",
                oldMaxLength: 10);

            //migrationBuilder.AlterColumn<string>(
            //    name: "Sex",
            //    table: "Patients",
            //    type: "longtext",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldType: "ENUM('Mascul','Femela')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "varchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "ENUM('manager','farmacist','asistent')",
                oldMaxLength: 10);

            //migrationBuilder.AlterColumn<string>(
            //    name: "Sex",
            //    table: "Patients",
            //    type: "ENUM('Mascul','Femela')",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldType: "longtext");
        }
    }
}
