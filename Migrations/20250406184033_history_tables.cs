using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace VetManagement.Migrations
{
    /// <inheritdoc />
    public partial class history_tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Owners");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Meds");

            migrationBuilder.CreateTable(
                name: "MedHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    OriginalId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    Type = table.Column<string>(type: "longtext", nullable: false),
                    MeasurmentUnit = table.Column<string>(type: "longtext", nullable: false),
                    PieceType = table.Column<string>(type: "longtext", nullable: false),
                    Pieces = table.Column<int>(type: "int", nullable: false),
                    PerPiece = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Provider = table.Column<string>(type: "longtext", nullable: false),
                    InvoiceNumber = table.Column<int>(type: "int", nullable: false),
                    WaitingTime = table.Column<string>(type: "longtext", nullable: false),
                    Administration = table.Column<string>(type: "longtext", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LotID = table.Column<string>(type: "longtext", nullable: true),
                    Valability = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DateDeleted = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastUsed = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedHistory", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OwnerHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    OriginalId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Street = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    StreetNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    Town = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    County = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    Phone = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false),
                    Email = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    Details = table.Column<string>(type: "longtext", nullable: true),
                    DateDeleted = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OwnerHistory", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PatientHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    OriginalId = table.Column<int>(type: "int", nullable: false),
                    OriginalOwnerId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "longtext", nullable: false),
                    Identifier = table.Column<int>(type: "int", nullable: true),
                    OwnerId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    Color = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    Species = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Sex = table.Column<string>(type: "longtext", nullable: false),
                    Race = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<float>(type: "float", nullable: false),
                    Details = table.Column<string>(type: "longtext", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DateDeleted = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientHistory", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TreatmentHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    OriginalId = table.Column<int>(type: "int", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    PatientType = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    PatientIdentifier = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    PatientSpecies = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    PatientRace = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    PatientSex = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    PatientAge = table.Column<int>(type: "int", nullable: false),
                    PatientWeight = table.Column<double>(type: "double", nullable: false),
                    OwnerId = table.Column<int>(type: "int", nullable: false),
                    OwnerName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    OwnerAddress = table.Column<string>(type: "varchar(400)", maxLength: 400, nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DateDeleted = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Details = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreatmentHistory", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TreatmentMedHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    TreatmentHistoryId = table.Column<int>(type: "int", nullable: false),
                    OriginalMedId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    Type = table.Column<string>(type: "longtext", nullable: false),
                    Quantity = table.Column<string>(type: "longtext", nullable: false),
                    WaitingTime = table.Column<string>(type: "longtext", nullable: false),
                    Administration = table.Column<string>(type: "longtext", nullable: false),
                    InvoiceNumber = table.Column<int>(type: "int", nullable: false),
                    LotID = table.Column<string>(type: "longtext", nullable: false),
                    Valability = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    MedId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreatmentMedHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TreatmentMedHistory_Meds_MedId",
                        column: x => x.MedId,
                        principalTable: "Meds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TreatmentMedHistory_TreatmentHistory_TreatmentHistoryId",
                        column: x => x.TreatmentHistoryId,
                        principalTable: "TreatmentHistory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentMedHistory_MedId",
                table: "TreatmentMedHistory",
                column: "MedId");

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentMedHistory_TreatmentHistoryId",
                table: "TreatmentMedHistory",
                column: "TreatmentHistoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedHistory");

            migrationBuilder.DropTable(
                name: "OwnerHistory");

            migrationBuilder.DropTable(
                name: "PatientHistory");

            migrationBuilder.DropTable(
                name: "TreatmentMedHistory");

            migrationBuilder.DropTable(
                name: "TreatmentHistory");

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

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Meds",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
