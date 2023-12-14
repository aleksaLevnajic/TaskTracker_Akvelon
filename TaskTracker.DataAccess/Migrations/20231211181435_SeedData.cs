using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TaskTracker.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "EndDate", "Name", "Priority", "StartDate", "Status" },
                values: new object[] { 1, null, "Task Tracker App", 1, new DateTime(2023, 12, 11, 18, 14, 34, 857, DateTimeKind.Utc).AddTicks(2434), 2 });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Description", "Name", "Priority", "ProjectId", "Status" },
                values: new object[,]
                {
                    { 1, "Present project.", "Initial meeting", 1, 1, 3 },
                    { 2, "Building software soultion for Task Tracker app.", "Develop App", 2, 1, 2 },
                    { 3, "Final pre-production meeting.", "Final meeting", 3, 1, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
