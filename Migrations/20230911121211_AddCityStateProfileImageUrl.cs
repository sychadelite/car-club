using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAppCarClub.Migrations
{
    public partial class AddCityStateProfileImageUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Addresses_AdressId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "AdressId",
                table: "AspNetUsers",
                newName: "AddressId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_AdressId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_AddressId");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileImageUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Addresses_AddressId",
                table: "AspNetUsers",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Addresses_AddressId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "City",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfileImageUrl",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "State",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "AddressId",
                table: "AspNetUsers",
                newName: "AdressId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_AddressId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_AdressId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Addresses_AdressId",
                table: "AspNetUsers",
                column: "AdressId",
                principalTable: "Addresses",
                principalColumn: "Id");
        }
    }
}
