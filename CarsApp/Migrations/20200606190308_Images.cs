using Microsoft.EntityFrameworkCore.Migrations;

namespace CarsApp.Migrations
{
    public partial class Images : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ManufacturerPicture",
                table: "Manufacturer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DriverPicture",
                table: "Driver",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CarPicture",
                table: "Car",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ManufacturerPicture",
                table: "Manufacturer");

            migrationBuilder.DropColumn(
                name: "DriverPicture",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "CarPicture",
                table: "Car");
        }
    }
}
