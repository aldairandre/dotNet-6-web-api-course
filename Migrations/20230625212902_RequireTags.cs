using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotNet_6_web_api_course.Migrations
{
    /// <inheritdoc />
    public partial class RequireTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Category",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Category");
        }
    }
}
