using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SoundShapesServer.Migrations
{
    /// <inheritdoc />
    public partial class AddIp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IpId",
                table: "Tokens",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Ips",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    IpAddress = table.Column<string>(type: "character varying(39)", maxLength: 39, nullable: false),
                    Country = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    CreationDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    AuthorizedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Authorized = table.Column<bool>(type: "boolean", nullable: false),
                    OneTimeUse = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ips_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_IpId",
                table: "Tokens",
                column: "IpId");

            migrationBuilder.CreateIndex(
                name: "IX_Ips_UserId",
                table: "Ips",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tokens_Ips_IpId",
                table: "Tokens",
                column: "IpId",
                principalTable: "Ips",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tokens_Ips_IpId",
                table: "Tokens");

            migrationBuilder.DropTable(
                name: "Ips");

            migrationBuilder.DropIndex(
                name: "IX_Tokens_IpId",
                table: "Tokens");

            migrationBuilder.DropColumn(
                name: "IpId",
                table: "Tokens");
        }
    }
}
