using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mediaverse.Infrastructure.Migrations
{
    public partial class FixedRoomViewersPrimaryKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomViewers_Rooms_RoomId",
                table: "RoomViewers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomViewers",
                table: "RoomViewers");

            migrationBuilder.AlterColumn<Guid>(
                name: "RoomId",
                table: "RoomViewers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomViewers",
                table: "RoomViewers",
                columns: new[] { "Id", "RoomId" });

            migrationBuilder.AddForeignKey(
                name: "FK_RoomViewers_Rooms_RoomId",
                table: "RoomViewers",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomViewers_Rooms_RoomId",
                table: "RoomViewers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomViewers",
                table: "RoomViewers");

            migrationBuilder.AlterColumn<Guid>(
                name: "RoomId",
                table: "RoomViewers",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomViewers",
                table: "RoomViewers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomViewers_Rooms_RoomId",
                table: "RoomViewers",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
