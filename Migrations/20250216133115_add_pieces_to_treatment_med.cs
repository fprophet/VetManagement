using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VetManagement.Migrations
{
    /// <inheritdoc />
    public partial class add_pieces_to_treatment_med : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Pieces",
                table: "TreatmentMed",
                type: "float",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pieces",
                table: "TreatmentMed");
        }
    }
}
