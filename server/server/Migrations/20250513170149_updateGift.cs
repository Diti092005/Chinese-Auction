using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    /// <inheritdoc />
    public partial class updateGift : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "details",
                table: "Gifts",
                newName: "Details");

            migrationBuilder.RenameColumn(
                name: "image",
                table: "Gifts",
                newName: "ImageUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Details",
                table: "Gifts",
                newName: "details");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Gifts",
                newName: "image");
        }
    }
}
