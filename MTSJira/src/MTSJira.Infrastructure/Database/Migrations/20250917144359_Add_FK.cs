using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MTSJira.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_FK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_tasks_priority_id",
                table: "tasks",
                column: "priority_id");

            migrationBuilder.CreateIndex(
                name: "IX_tasks_status_id",
                table: "tasks",
                column: "status_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tasks_task_priorities_priority_id",
                table: "tasks",
                column: "priority_id",
                principalTable: "task_priorities",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tasks_task_statuses_status_id",
                table: "tasks",
                column: "status_id",
                principalTable: "task_statuses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tasks_task_priorities_priority_id",
                table: "tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_tasks_task_statuses_status_id",
                table: "tasks");

            migrationBuilder.DropIndex(
                name: "IX_tasks_priority_id",
                table: "tasks");

            migrationBuilder.DropIndex(
                name: "IX_tasks_status_id",
                table: "tasks");
        }
    }
}
