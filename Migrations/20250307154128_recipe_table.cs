using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace VetManagement.Migrations
{
    /// <inheritdoc />
    public partial class recipe_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecipeDate",
                table: "RegistryRecords");

            migrationBuilder.DropColumn(
                name: "RecipeNumber",
                table: "RegistryRecords");

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<long>(type: "bigint", nullable: false),
                    TreatmentId = table.Column<int>(type: "int", nullable: false),
                    RegistryNumber = table.Column<int>(type: "int", nullable: false),
                    MedName = table.Column<string>(type: "longtext", nullable: false),
                    Signed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    OwnerSignature = table.Column<string>(type: "longtext", nullable: false),
                    RegistryRecordId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recipes_RegistryRecords_RegistryRecordId",
                        column: x => x.RegistryRecordId,
                        principalTable: "RegistryRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_RegistryRecordId",
                table: "Recipes",
                column: "RegistryRecordId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Recipes");

            migrationBuilder.AddColumn<int>(
                name: "RecipeDate",
                table: "RegistryRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RecipeNumber",
                table: "RegistryRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
