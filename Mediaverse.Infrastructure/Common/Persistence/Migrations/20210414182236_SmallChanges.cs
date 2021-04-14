using Microsoft.EntityFrameworkCore.Migrations;

namespace Mediaverse.Infrastructure.Common.Persistence.Migrations
{
    public partial class SmallChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistItems_Playlists_PlaylistId",
                table: "PlaylistItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CachedContent",
                table: "CachedContent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlaylistItems",
                table: "PlaylistItems");

            migrationBuilder.RenameTable(
                name: "PlaylistItems",
                newName: "PlaylistItems");

            migrationBuilder.RenameColumn(
                name: "Host",
                table: "Rooms",
                newName: "HostId");

            migrationBuilder.RenameIndex(
                name: "IX_PlaylistItems_PlaylistId",
                table: "PlaylistItems",
                newName: "IX_PlaylistItems_PlaylistId");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "CurrentlyPlayingContentIndex",
                table: "Playlists",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CachedContent",
                table: "CachedContent",
                columns: new[] { "ExternalId", "ContentSource", "ContentType" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlaylistItems",
                table: "PlaylistItems",
                columns: new[] { "ExternalId", "ContentSource", "ContentType" });

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistItems_Playlists_PlaylistId",
                table: "PlaylistItems",
                column: "PlaylistId",
                principalTable: "Playlists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistItems_Playlists_PlaylistId",
                table: "PlaylistItems");
            
            migrationBuilder.DropPrimaryKey(
                name: "PK_CachedContent",
                table: "CachedContent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlaylistItems",
                table: "PlaylistItems");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Rooms");

            migrationBuilder.RenameTable(
                name: "PlaylistItems",
                newName: "PlaylistItems");

            migrationBuilder.RenameColumn(
                name: "HostId",
                table: "Rooms",
                newName: "Host");

            migrationBuilder.RenameIndex(
                name: "IX_PlaylistItems_PlaylistId",
                table: "PlaylistItems",
                newName: "IX_PlaylistItemDto_PlaylistId");

            migrationBuilder.AlterColumn<int>(
                name: "CurrentlyPlayingContentIndex",
                table: "Playlists",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CachedContent",
                table: "CachedContent",
                columns: new[] { "ExternalId", "ContentType", "ContentSource" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlaylistItemDto",
                table: "PlaylistItemDto",
                columns: new[] { "ExternalId", "ContentType", "ContentSource" });

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistItems_Playlists_PlaylistId",
                table: "PlaylistItems",
                column: "PlaylistDtoId",
                principalTable: "Playlists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
