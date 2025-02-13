using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventHorizon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueEntryIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EventEntries_EventId",
                table: "EventEntries");

            migrationBuilder.CreateIndex(
                name: "IX_EventEntries_EventId_UserId",
                table: "EventEntries",
                columns: new[] { "EventId", "UserId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EventEntries_EventId_UserId",
                table: "EventEntries");

            migrationBuilder.CreateIndex(
                name: "IX_EventEntries_EventId",
                table: "EventEntries",
                column: "EventId");
        }
    }
}
