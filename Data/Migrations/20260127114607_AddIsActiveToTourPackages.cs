using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIsActiveToTourPackages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "TourPackages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "TourPackages");
        }
    }
}
