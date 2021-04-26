using Microsoft.EntityFrameworkCore.Migrations;

namespace Mediaverse.Infrastructure.Common.Persistence.Migrations
{
    public partial class AddThumbnail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ThumbnailHeight",
                table: "CachedContent",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "ThumbnailUrl",
                table: "CachedContent",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ThumbnailWidth",
                table: "CachedContent",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThumbnailHeight",
                table: "CachedContent");

            migrationBuilder.DropColumn(
                name: "ThumbnailUrl",
                table: "CachedContent");

            migrationBuilder.DropColumn(
                name: "ThumbnailWidth",
                table: "CachedContent");
        }
    }
}
