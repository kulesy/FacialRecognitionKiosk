using Microsoft.EntityFrameworkCore.Migrations;

namespace DsReceptionAPI.Migrations
{
    public partial class ClientField_EnabledFaceIdentification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EnabledFaceIdentification",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnabledFaceIdentification",
                table: "AspNetUsers");
        }
    }
}
