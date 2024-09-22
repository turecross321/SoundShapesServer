using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SoundShapesServer.Migrations
{
    /// <inheritdoc />
    public partial class SwitchToIpGuids : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Remove all tokens and ips
            migrationBuilder.Sql("DELETE FROM \"Tokens\";");
            migrationBuilder.Sql("DELETE FROM \"Ips\";");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // If necessary, you can implement the logic to restore data here
            // For example, if you want to add back the entries that were deleted,
            // you may need to have a backup mechanism.
        }
    }
}
