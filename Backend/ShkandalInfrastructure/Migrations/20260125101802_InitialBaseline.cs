using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ShkandalInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialBaseline : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "app_user");

            migrationBuilder.DropTable(
                name: "article");

            migrationBuilder.DropTable(
                name: "categoryCluster");

            migrationBuilder.DropTable(
                name: "media");

            migrationBuilder.DropTable(
                name: "category");

            migrationBuilder.DropTable(
                name: "cluster");
        }
    }
}
