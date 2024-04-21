using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject.Migrations
{
    /// <inheritdoc />
    public partial class a7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "like",
                table: "Posts",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "likes",
                table: "Posts",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "like",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "likes",
                table: "Posts");
        }
    }
}
