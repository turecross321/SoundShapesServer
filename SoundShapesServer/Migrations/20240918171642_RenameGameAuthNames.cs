using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoundShapesServer.Migrations
{
    /// <inheritdoc />
    public partial class RenameGameAuthNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AllowRpcnAuthentication",
                table: "Users",
                newName: "RpcnAuthorization");

            migrationBuilder.RenameColumn(
                name: "AllowPsnAuthentication",
                table: "Users",
                newName: "PsnAuthorization");

            migrationBuilder.RenameColumn(
                name: "AllowIpAuthentication",
                table: "Users",
                newName: "IpAuthorization");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RpcnAuthorization",
                table: "Users",
                newName: "AllowRpcnAuthentication");

            migrationBuilder.RenameColumn(
                name: "PsnAuthorization",
                table: "Users",
                newName: "AllowPsnAuthentication");

            migrationBuilder.RenameColumn(
                name: "IpAuthorization",
                table: "Users",
                newName: "AllowIpAuthentication");
        }
    }
}
