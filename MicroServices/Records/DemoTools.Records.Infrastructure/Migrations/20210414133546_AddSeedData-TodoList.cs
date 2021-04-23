using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DemoTools.Records.Infrastructure.Migrations
{
    public partial class AddSeedDataTodoList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TodoList",
                columns: new[] { "TodoListID", "SubscriptionID", "Title" },
                values: new object[,]
                {
                    { new Guid("207df05f-ca31-40ad-8895-0800009398eb"), new Guid("831b3626-6359-4f56-b5c2-81d86efcdc55"), "New List - 1" },
                    { new Guid("27b9efe2-3f8d-4685-9ef9-a17deb4de2a0"), new Guid("831b3626-6359-4f56-b5c2-81d86efcdc55"), "New List - 2" }
                });

            migrationBuilder.InsertData(
                table: "TodoItem",
                columns: new[] { "TodoItemID", "TodoListID", "IsDone", "Title" },
                values: new object[,]
                {
                    { new Guid("5dea32f8-b10f-4ddf-a49b-8b65f77e004d"), new Guid("207df05f-ca31-40ad-8895-0800009398eb"), false, "TodoItem - 1.1" },
                    { new Guid("c7eb2bf8-cbe2-4425-b136-f4c56bdb69d9"), new Guid("27b9efe2-3f8d-4685-9ef9-a17deb4de2a0"), false, "TodoItem - 2.1" },
                    { new Guid("47f66d75-354b-4a1c-9738-45daf0aed4b5"), new Guid("27b9efe2-3f8d-4685-9ef9-a17deb4de2a0"), true, "TodoItem - 2.2" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TodoItem",
                keyColumns: new[] { "TodoItemID", "TodoListID" },
                keyValues: new object[] { new Guid("47f66d75-354b-4a1c-9738-45daf0aed4b5"), new Guid("27b9efe2-3f8d-4685-9ef9-a17deb4de2a0") });

            migrationBuilder.DeleteData(
                table: "TodoItem",
                keyColumns: new[] { "TodoItemID", "TodoListID" },
                keyValues: new object[] { new Guid("5dea32f8-b10f-4ddf-a49b-8b65f77e004d"), new Guid("207df05f-ca31-40ad-8895-0800009398eb") });

            migrationBuilder.DeleteData(
                table: "TodoItem",
                keyColumns: new[] { "TodoItemID", "TodoListID" },
                keyValues: new object[] { new Guid("c7eb2bf8-cbe2-4425-b136-f4c56bdb69d9"), new Guid("27b9efe2-3f8d-4685-9ef9-a17deb4de2a0") });

            migrationBuilder.DeleteData(
                table: "TodoList",
                keyColumn: "TodoListID",
                keyValue: new Guid("207df05f-ca31-40ad-8895-0800009398eb"));

            migrationBuilder.DeleteData(
                table: "TodoList",
                keyColumn: "TodoListID",
                keyValue: new Guid("27b9efe2-3f8d-4685-9ef9-a17deb4de2a0"));
        }
    }
}
