using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VetManagement.Migrations
{
    /// <inheritdoc />
    public partial class owner_patient_columns_to_Treatments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Administration",
                table: "TreatmentMed");

            migrationBuilder.DropColumn(
                name: "Pieces",
                table: "TreatmentMed");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "varchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "ENUM('manager','farmacist','asistent')",
                oldMaxLength: 10);

            migrationBuilder.AddColumn<string>(
                name: "OwnerAddress",
                table: "Treatments",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "PatientAge",
                table: "Treatments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "PatientWeight",
                table: "Treatments",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Valability",
                table: "TreatmentMed",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Patients",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Owners",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Administration",
                table: "Meds",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Meds",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerAddress",
                table: "Treatments");

            migrationBuilder.DropColumn(
                name: "PatientAge",
                table: "Treatments");

            migrationBuilder.DropColumn(
                name: "PatientWeight",
                table: "Treatments");

            migrationBuilder.DropColumn(
                name: "Valability",
                table: "TreatmentMed");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Owners");

            migrationBuilder.DropColumn(
                name: "Administration",
                table: "Meds");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Meds");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "ENUM('manager','farmacist','asistent')",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AddColumn<string>(
                name: "Administration",
                table: "TreatmentMed",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Pieces",
                table: "TreatmentMed",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
