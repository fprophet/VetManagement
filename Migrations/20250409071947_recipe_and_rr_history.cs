using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace VetManagement.Migrations
{
    /// <inheritdoc />
    public partial class recipe_and_rr_history : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RecipeHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    OriginalId = table.Column<int>(type: "int", nullable: false),
                    OriginalRegistryNumber = table.Column<int>(type: "int", nullable: false),
                    HistoryRegistryNumber = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SignedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    MedName = table.Column<string>(type: "longtext", nullable: false),
                    Signed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    OwnerSignature = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeHistory", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RegistryRecordHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    OriginalId = table.Column<int>(type: "int", nullable: false),
                    OriginalTreatmentId = table.Column<int>(type: "int", nullable: false),
                    HistoryTreatmentId = table.Column<int>(type: "int", nullable: false),
                    OriginalRecipeNumber = table.Column<int>(type: "int", nullable: false),
                    HistoryRecipeNumber = table.Column<int>(type: "int", nullable: false),
                    TreatmentDuration = table.Column<string>(type: "longtext", nullable: false),
                    Outcome = table.Column<string>(type: "longtext", nullable: false),
                    MedName = table.Column<string>(type: "longtext", nullable: false),
                    Observations = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistryRecordHistory", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipeHistory");

            migrationBuilder.DropTable(
                name: "RegistryRecordHistory");
        }
    }
}
