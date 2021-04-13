using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mediaverse.Infrastructure.Common.Persistence.Migrations
{
    public partial class AddRoom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Host = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActivePlaylistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaxViewersQuantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoomViewers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomViewers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomViewers_Rooms_Id",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoomViewers_Id",
                table: "RoomViewers",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomViewers");

            migrationBuilder.DropTable(
                name: "Rooms");
        }
    }
}
