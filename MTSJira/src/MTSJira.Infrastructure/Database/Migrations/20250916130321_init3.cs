using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MTSJira.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class init3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RelationshipType",
                table: "TaskRelationships");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RelationshipType",
                table: "TaskRelationships",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
