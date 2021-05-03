using Microsoft.EntityFrameworkCore.Migrations;

namespace Mediaverse.Infrastructure.Migrations
{
    public partial class UpdatedCurrentContent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentlyPlayingContentIndex",
                table: "Playlists");

            migrationBuilder.RenameColumn(
                name: "PlayingState",
                table: "CurrentRoomsContent",
                newName: "PlayerState");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PlayerState",
                table: "CurrentRoomsContent",
                newName: "PlayingState");

            migrationBuilder.AddColumn<int>(
                name: "CurrentlyPlayingContentIndex",
                table: "Playlists",
                type: "int",
                nullable: true);
        }
    }
}
