using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoundShapesServer.Migrations
{
    /// <inheritdoc />
    public partial class CascadeDeletionRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tokens_RefreshTokens_RefreshTokenId",
                table: "Tokens");

            migrationBuilder.AddForeignKey(
                name: "FK_Tokens_RefreshTokens_RefreshTokenId",
                table: "Tokens",
                column: "RefreshTokenId",
                principalTable: "RefreshTokens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tokens_RefreshTokens_RefreshTokenId",
                table: "Tokens");

            migrationBuilder.AddForeignKey(
                name: "FK_Tokens_RefreshTokens_RefreshTokenId",
                table: "Tokens",
                column: "RefreshTokenId",
                principalTable: "RefreshTokens",
                principalColumn: "Id");
        }
    }
}
