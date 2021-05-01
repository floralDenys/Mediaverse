using Microsoft.EntityFrameworkCore.Migrations;

namespace Mediaverse.Infrastructure.Migrations
{
    public partial class UpdatedCurrentContent2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "PlayingTime",
                table: "CurrentRoomsContent",
                type: "float",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "PlayingTime",
                table: "CurrentRoomsContent",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
