using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShedulingReminders.Infrastructure.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class Initialize_14052023140813 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Reminders",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reminders_AppUserId",
                table: "Reminders",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reminders_AspNetUsers_AppUserId",
                table: "Reminders",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reminders_AspNetUsers_AppUserId",
                table: "Reminders");

            migrationBuilder.DropIndex(
                name: "IX_Reminders_AppUserId",
                table: "Reminders");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Reminders");
        }
    }
}
