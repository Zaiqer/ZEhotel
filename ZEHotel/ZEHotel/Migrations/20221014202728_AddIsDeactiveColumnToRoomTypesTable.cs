using Microsoft.EntityFrameworkCore.Migrations;

namespace ZEHotel.Migrations
{
    public partial class AddIsDeactiveColumnToRoomTypesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeactive",
                table: "RoomTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeactive",
                table: "RoomTypes");
        }
    }
}
