using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    /// <inheritdoc />
    public partial class ChangeWinnerId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserWinnerId",
                table: "Gifts");

            migrationBuilder.AddColumn<string>(
                name: "Fullname",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fullname",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "UserWinnerId",
                table: "Gifts",
                type: "int",
                nullable: true);
        }
    }
}
