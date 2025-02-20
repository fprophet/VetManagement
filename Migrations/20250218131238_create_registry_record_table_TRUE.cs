using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace VetManagement.Migrations
{
    /// <inheritdoc />
    public partial class create_registry_record_table_TRUE : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RegistryRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    OwnerId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<long>(type: "bigint", nullable: false),
                    Species = table.Column<string>(type: "longtext", nullable: false),
                    Sex = table.Column<string>(type: "longtext", nullable: false),
                    Age = table.Column<float>(type: "float", nullable: false),
                    Identifier = table.Column<string>(type: "longtext", nullable: false),
                    Symptoms = table.Column<string>(type: "longtext", nullable: false),
                    RecipeNumber = table.Column<int>(type: "int", nullable: false),
                    RecipeDate = table.Column<int>(type: "int", nullable: false),
                    TreatmentDuration = table.Column<string>(type: "longtext", nullable: false),
                    Outcome = table.Column<string>(type: "longtext", nullable: false),
                    MedName = table.Column<string>(type: "longtext", nullable: false),
                    Observations = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistryRecords", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegistryRecords");
        }
    }
}
