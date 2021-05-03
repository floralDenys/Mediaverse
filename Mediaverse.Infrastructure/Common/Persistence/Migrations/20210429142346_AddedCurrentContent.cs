using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mediaverse.Infrastructure.Migrations
{
    public partial class AddedCurrentContent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CurrentRoomsContent",
                columns: table => new
                {
                    RoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExternalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Source = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    PlayingState = table.Column<int>(type: "int", nullable: false),
                    PlayingTime = table.Column<long>(type: "bigint", nullable: false),
                    LastUpdatedPlayingTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrentRoomsContent", x => x.RoomId);
                    table.ForeignKey(
                        name: "FK_CurrentRoomsContent_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrentRoomsContent");
        }
    }
}
