using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MTSJira.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "task_priorities",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task_priorities", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "task_statuses",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task_statuses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tasks",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    status_id = table.Column<int>(type: "integer", nullable: false),
                    priority_id = table.Column<int>(type: "integer", nullable: false),
                    author = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    assignee = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    parent_task_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tasks", x => x.id);
                    table.ForeignKey(
                        name: "FK_tasks_tasks_parent_task_id",
                        column: x => x.parent_task_id,
                        principalTable: "tasks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tasks_relationships",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    source_task_id = table.Column<int>(type: "integer", nullable: false),
                    related_task_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tasks_relationships", x => x.id);
                    table.ForeignKey(
                        name: "FK_tasks_relationships_tasks_related_task_id",
                        column: x => x.related_task_id,
                        principalTable: "tasks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tasks_relationships_tasks_source_task_id",
                        column: x => x.source_task_id,
                        principalTable: "tasks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "task_priorities",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 0, "Low" },
                    { 1, "Medium" },
                    { 2, "High" }
                });

            migrationBuilder.InsertData(
                table: "task_statuses",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 0, "New" },
                    { 1, "InProgress" },
                    { 2, "Done" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_tasks_parent_task_id",
                table: "tasks",
                column: "parent_task_id");

            migrationBuilder.CreateIndex(
                name: "IX_tasks_relationships_related_task_id",
                table: "tasks_relationships",
                column: "related_task_id");

            migrationBuilder.CreateIndex(
                name: "IX_tasks_relationships_source_task_id",
                table: "tasks_relationships",
                column: "source_task_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "task_priorities");

            migrationBuilder.DropTable(
                name: "task_statuses");

            migrationBuilder.DropTable(
                name: "tasks_relationships");

            migrationBuilder.DropTable(
                name: "tasks");
        }
    }
}
