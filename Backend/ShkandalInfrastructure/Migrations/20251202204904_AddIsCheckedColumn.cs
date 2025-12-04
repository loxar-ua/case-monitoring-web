using Microsoft.EntityFrameworkCore.Migrations;
using Pgvector;

#nullable disable

namespace ShkandalInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsCheckedColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_checked",
                table: "article",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_checked",
                table: "article");
        }
    }
}
