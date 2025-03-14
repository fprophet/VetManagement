using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VetManagement.Migrations
{
    /// <inheritdoc />
    public partial class rr_recipe_one_to_one_rel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
 

       
            migrationBuilder.CreateIndex(
                name: "IX_RegistryRecords_RecipeNumber",
                table: "RegistryRecords",
                column: "RecipeNumber",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RegistryRecords_Recipes_RecipeNumber",
                table: "RegistryRecords",
                column: "RecipeNumber",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegistryRecords_Recipes_RecipeNumber",
                table: "RegistryRecords");

            migrationBuilder.DropIndex(
                name: "IX_RegistryRecords_RecipeNumber",
                table: "RegistryRecords");

        }
    }
}
