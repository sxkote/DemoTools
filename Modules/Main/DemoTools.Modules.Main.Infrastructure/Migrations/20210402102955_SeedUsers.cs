using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DemoTools.Modules.Main.Infrastructure.Migrations
{
    public partial class SeedUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "PersonID", "BirthDate", "Cellular", "Email", "Gender", "NameFirst", "NameLast", "NameSecond" },
                values: new object[] { new Guid("37846734-172e-4149-8cec-6f43d1eb3f60"), null, "+7-909-900-7011", "ivan@litskevich.ru", (byte)1, "John", "Doe", "" });

            migrationBuilder.InsertData(
                table: "TypesUserPermission",
                columns: new[] { "UserPermissionID", "Category", "Name", "Title" },
                values: new object[] { new Guid("bf87080f-e529-4520-885f-29f492fe63fe"), "001", "Login", "Login" });

            migrationBuilder.InsertData(
                table: "TypesUserRole",
                columns: new[] { "UserRoleID", "Name", "Title" },
                values: new object[,]
                {
                    { new Guid("45e7071e-85c2-4be2-8594-505aae4bf0d1"), "Administrator", "Administrator" },
                    { new Guid("6b823d5e-949f-4be5-926b-8dde77112d0f"), "User", "User" }
                });

            migrationBuilder.InsertData(
                table: "TypesUserRolePermission",
                columns: new[] { "UserPermissionID", "UserRoleID" },
                values: new object[] { new Guid("bf87080f-e529-4520-885f-29f492fe63fe"), new Guid("6b823d5e-949f-4be5-926b-8dde77112d0f") });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "Login", "Password" },
                values: new object[] { new Guid("37846734-172e-4149-8cec-6f43d1eb3f60"), "guest", "123456" });

            migrationBuilder.InsertData(
                table: "UsersRoles",
                columns: new[] { "UserID", "UserRoleID" },
                values: new object[,]
                {
                    { new Guid("37846734-172e-4149-8cec-6f43d1eb3f60"), new Guid("45e7071e-85c2-4be2-8594-505aae4bf0d1") },
                    { new Guid("37846734-172e-4149-8cec-6f43d1eb3f60"), new Guid("6b823d5e-949f-4be5-926b-8dde77112d0f") }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TypesUserRolePermission",
                keyColumns: new[] { "UserPermissionID", "UserRoleID" },
                keyValues: new object[] { new Guid("bf87080f-e529-4520-885f-29f492fe63fe"), new Guid("6b823d5e-949f-4be5-926b-8dde77112d0f") });

            migrationBuilder.DeleteData(
                table: "UsersRoles",
                keyColumns: new[] { "UserID", "UserRoleID" },
                keyValues: new object[] { new Guid("37846734-172e-4149-8cec-6f43d1eb3f60"), new Guid("45e7071e-85c2-4be2-8594-505aae4bf0d1") });

            migrationBuilder.DeleteData(
                table: "UsersRoles",
                keyColumns: new[] { "UserID", "UserRoleID" },
                keyValues: new object[] { new Guid("37846734-172e-4149-8cec-6f43d1eb3f60"), new Guid("6b823d5e-949f-4be5-926b-8dde77112d0f") });

            migrationBuilder.DeleteData(
                table: "TypesUserPermission",
                keyColumn: "UserPermissionID",
                keyValue: new Guid("bf87080f-e529-4520-885f-29f492fe63fe"));

            migrationBuilder.DeleteData(
                table: "TypesUserRole",
                keyColumn: "UserRoleID",
                keyValue: new Guid("45e7071e-85c2-4be2-8594-505aae4bf0d1"));

            migrationBuilder.DeleteData(
                table: "TypesUserRole",
                keyColumn: "UserRoleID",
                keyValue: new Guid("6b823d5e-949f-4be5-926b-8dde77112d0f"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: new Guid("37846734-172e-4149-8cec-6f43d1eb3f60"));

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "PersonID",
                keyValue: new Guid("37846734-172e-4149-8cec-6f43d1eb3f60"));
        }
    }
}
