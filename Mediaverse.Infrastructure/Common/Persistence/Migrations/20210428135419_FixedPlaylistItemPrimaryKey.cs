using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mediaverse.Infrastructure.Migrations
{
    public partial class FixedPlaylistItemPrimaryKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistItems_Playlists_PlaylistId",
                table: "PlaylistItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlaylistItems",
                table: "PlaylistItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "PlaylistId",
                table: "PlaylistItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlaylistItems",
                table: "PlaylistItems",
                columns: new[] { "ExternalId", "ContentSource", "ContentType", "PlaylistId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistItems_Playlists_PlaylistId",
                table: "PlaylistItems",
                column: "PlaylistId",
                principalTable: "Playlists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistItems_Playlists_PlaylistId",
                table: "PlaylistItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlaylistItems",
                table: "PlaylistItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "PlaylistId",
                table: "PlaylistItems",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

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
    }
}
