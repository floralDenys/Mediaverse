using Microsoft.EntityFrameworkCore.Migrations;

namespace Mediaverse.Infrastructure.Common.Persistence.Migrations
{
    public partial class AddedNameToPlaylist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Playlists",
                type: "nvarchar(max)",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Playlists");
        }
    }
}
