using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject.Migrations
{
    /// <inheritdoc />
    public partial class a5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "image",
                table: "Posts",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "text",
                table: "Posts",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "video",
                table: "Posts",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "image",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "text",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "video",
                table: "Posts");
        }
    }
}
