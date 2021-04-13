using Microsoft.EntityFrameworkCore.Migrations;

namespace Mediaverse.Infrastructure.Common.Persistence.Migrations
{
    public partial class AddContent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CachedContent",
                columns: table => new
                {
                    ExternalId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ContentSource = table.Column<int>(type: "int", nullable: false),
                    ContentType = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentPlayerHtml = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentPlayerWidth = table.Column<int>(type: "int", nullable: false),
                    ContentPlayerHeight = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CachedContent", x => new { x.ExternalId, x.ContentType, x.ContentSource });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CachedContent");
        }
    }
}
