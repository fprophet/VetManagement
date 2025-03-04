using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VetManagement.Migrations
{
    /// <inheritdoc />
    public partial class med_measurment_unit_colum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<string>(
            //    name: "MeasurmentUnit",
            //    table: "Meds",
            //    type: "longtext",
            //    nullable: false);

            //migrationBuilder.AlterColumn<long>(
            //    name: "Date",
            //    table: "Invoices",
            //    type: "bigint",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MeasurmentUnit",
                table: "Meds");

            migrationBuilder.AlterColumn<int>(
                name: "Date",
                table: "Invoices",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
