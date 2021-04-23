using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DemoTools.Authorization.Infrastructure.Migrations
{
    public partial class InitMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Activity",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    PersonID = table.Column<Guid>(type: "uuid", nullable: true),
                    Date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Expire = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Type = table.Column<string>(type: "character varying(100)", fixedLength: true, maxLength: 100, nullable: true),
                    Data = table.Column<string>(type: "text", nullable: true),
                    Pin = table.Column<string>(type: "character varying(20)", fixedLength: true, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activity", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AuthToken",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    UserID = table.Column<Guid>(type: "uuid", nullable: false),
                    Expire = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Data = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthToken", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    PersonID = table.Column<Guid>(type: "uuid", nullable: false),
                    SubscriptionID = table.Column<Guid>(type: "uuid", nullable: false),
                    NameFirst = table.Column<string>(type: "character varying(100)", fixedLength: true, maxLength: 100, nullable: true),
                    NameLast = table.Column<string>(type: "character varying(100)", fixedLength: true, maxLength: 100, nullable: true),
                    NameSecond = table.Column<string>(type: "character varying(100)", fixedLength: true, maxLength: 100, nullable: true),
                    Cellular = table.Column<string>(type: "character varying(100)", fixedLength: true, maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "character varying(100)", fixedLength: true, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.PersonID);
                });

            migrationBuilder.CreateTable(
                name: "TypesUserPermission",
                columns: table => new
                {
                    UserPermissionID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", fixedLength: true, maxLength: 100, nullable: true),
                    Title = table.Column<string>(type: "character varying(100)", fixedLength: true, maxLength: 100, nullable: true),
                    Category = table.Column<string>(type: "character varying(100)", fixedLength: true, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypesUserPermission", x => x.UserPermissionID);
                });

            migrationBuilder.CreateTable(
                name: "TypesUserRole",
                columns: table => new
                {
                    UserRoleID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", fixedLength: true, maxLength: 100, nullable: true),
                    Title = table.Column<string>(type: "character varying(100)", fixedLength: true, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypesUserRole", x => x.UserRoleID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<Guid>(type: "uuid", nullable: false),
                    Login = table.Column<string>(type: "character varying(100)", fixedLength: true, maxLength: 100, nullable: true),
                    Password = table.Column<string>(type: "character varying(100)", fixedLength: true, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_Users_Persons_UserID",
                        column: x => x.UserID,
                        principalTable: "Persons",
                        principalColumn: "PersonID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TypesUserRolePermission",
                columns: table => new
                {
                    UserRoleID = table.Column<Guid>(type: "uuid", nullable: false),
                    UserPermissionID = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypesUserRolePermission", x => new { x.UserRoleID, x.UserPermissionID });
                    table.ForeignKey(
                        name: "FK_TypesUserRolePermission_TypesUserPermission_UserPermissionID",
                        column: x => x.UserPermissionID,
                        principalTable: "TypesUserPermission",
                        principalColumn: "UserPermissionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TypesUserRolePermission_TypesUserRole_UserRoleID",
                        column: x => x.UserRoleID,
                        principalTable: "TypesUserRole",
                        principalColumn: "UserRoleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsersRoles",
                columns: table => new
                {
                    UserID = table.Column<Guid>(type: "uuid", nullable: false),
                    UserRoleID = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersRoles", x => new { x.UserID, x.UserRoleID });
                    table.ForeignKey(
                        name: "FK_UsersRoles_TypesUserRole_UserRoleID",
                        column: x => x.UserRoleID,
                        principalTable: "TypesUserRole",
                        principalColumn: "UserRoleID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersRoles_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "PersonID", "Cellular", "Email", "SubscriptionID", "NameFirst", "NameLast", "NameSecond" },
                values: new object[] { new Guid("37846734-172e-4149-8cec-6f43d1eb3f60"), "+7-909-900-7011", "ivan@litskevich.ru", new Guid("831b3626-6359-4f56-b5c2-81d86efcdc55"), "John", "Doe", "" });

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

            migrationBuilder.CreateIndex(
                name: "IX_TypesUserRolePermission_UserPermissionID",
                table: "TypesUserRolePermission",
                column: "UserPermissionID");

            migrationBuilder.CreateIndex(
                name: "IX_UsersRoles_UserRoleID",
                table: "UsersRoles",
                column: "UserRoleID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activity");

            migrationBuilder.DropTable(
                name: "AuthToken");

            migrationBuilder.DropTable(
                name: "TypesUserRolePermission");

            migrationBuilder.DropTable(
                name: "UsersRoles");

            migrationBuilder.DropTable(
                name: "TypesUserPermission");

            migrationBuilder.DropTable(
                name: "TypesUserRole");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Persons");
        }
    }
}
