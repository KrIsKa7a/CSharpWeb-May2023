using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskBoardApp.Data.Migrations
{
    public partial class AddTasksAndBoardsAndSeedDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Boards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BoardId = table.Column<int>(type: "int", nullable: false),
                    OwnerId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tasks_Boards_BoardId",
                        column: x => x.BoardId,
                        principalTable: "Boards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Boards",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Open" });

            migrationBuilder.InsertData(
                table: "Boards",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "In Progress" });

            migrationBuilder.InsertData(
                table: "Boards",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "Done" });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "BoardId", "CreatedOn", "Description", "OwnerId", "Title" },
                values: new object[,]
                {
                    { new Guid("1cf42730-6a04-4fc0-8f4d-e8d8f5c53fcb"), 3, new DateTime(2022, 6, 7, 17, 34, 37, 328, DateTimeKind.Utc).AddTicks(5065), "Implement [Create Task] page for adding tasks", "42f89c0a-f6a3-426a-9340-f975127adb9d", "Create Tasks" },
                    { new Guid("75b13e19-ac25-4398-b596-69d9b7e67050"), 1, new DateTime(2023, 1, 7, 17, 34, 37, 328, DateTimeKind.Utc).AddTicks(5053), "Create Android client App for the RESTful TaskBoard service", "fda5f9e6-baa9-468c-8b85-0761d3266ce5", "Android Client App" },
                    { new Guid("aa748e8a-1d35-4ce4-bb07-17b250ab64b9"), 1, new DateTime(2022, 11, 19, 17, 34, 37, 328, DateTimeKind.Utc).AddTicks(5008), "Implement better styling for all public pages", "42f89c0a-f6a3-426a-9340-f975127adb9d", "Improve CSS styles" },
                    { new Guid("c6547bb4-39fe-445f-9fd4-f90c3c7b361e"), 2, new DateTime(2023, 5, 7, 17, 34, 37, 328, DateTimeKind.Utc).AddTicks(5061), "Create Desktop client App for the RESTful TaskBoard service", "42f89c0a-f6a3-426a-9340-f975127adb9d", "Desktop Client App" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_BoardId",
                table: "Tasks",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_OwnerId",
                table: "Tasks",
                column: "OwnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Boards");
        }
    }
}
