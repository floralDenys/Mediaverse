using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mediaverse.Infrastructure.Common.Persistence.Migrations
{
    public partial class FixRoomViewers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_RoomViewers_Id",
                table: "RoomViewers",
                newName: "IX_RoomViewers_RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomViewers_Rooms_RoomId",
                table: "RoomViewers",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomViewers_Rooms_RoomId",
                table: "RoomViewers");

            migrationBuilder.RenameIndex(
                name: "IX_RoomViewers_RoomId",
                table: "RoomViewers",
                newName: "IX_RoomViewers_Id");
        }
    }
}
