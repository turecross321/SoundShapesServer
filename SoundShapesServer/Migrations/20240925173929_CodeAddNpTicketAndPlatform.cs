using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoundShapesServer.Migrations
{
    /// <inheritdoc />
    public partial class CodeAddNpTicketAndPlatform : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "GenuineNpTicket",
                table: "Codes",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Platform",
                table: "Codes",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GenuineNpTicket",
                table: "Codes");

            migrationBuilder.DropColumn(
                name: "Platform",
                table: "Codes");
        }
    }
}
