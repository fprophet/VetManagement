using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VetManagement.Migrations
{
    /// <inheritdoc />
    public partial class add_foreing_invoice_key_to_meds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            //migrationBuilder.AlterColumn<int>(
            //    name: "Date",
            //    table: "Invoices",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime(6)");

            migrationBuilder.CreateIndex(
                name: "IX_Meds_InvoiceNumber",
                table: "Meds",
                column: "InvoiceNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_Meds_Invoices_InvoiceNumber",
                table: "Meds",
                column: "InvoiceNumber",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meds_Invoices_InvoiceNumber",
                table: "Meds");

            migrationBuilder.DropIndex(
                name: "IX_Meds_InvoiceNumber",
                table: "Meds");

            migrationBuilder.AddColumn<int>(
                name: "InvoiceId",
                table: "Meds",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Invoices",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Meds_InvoiceId",
                table: "Meds",
                column: "InvoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Meds_Invoices_InvoiceId",
                table: "Meds",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id");
        }
    }
}
