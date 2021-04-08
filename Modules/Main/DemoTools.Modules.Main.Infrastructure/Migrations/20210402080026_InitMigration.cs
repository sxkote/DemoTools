using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DemoTools.Modules.Main.Infrastructure.Migrations
{
    public partial class InitMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    NameFirst = table.Column<string>(type: "text", nullable: true),
                    NameLast = table.Column<string>(type: "text", nullable: true),
                    NameSecond = table.Column<string>(type: "text", nullable: true),
                    Gender = table.Column<byte>(type: "smallint", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Cellular = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.PersonID);
                });

            migrationBuilder.CreateTable(
                name: "TodoList",
                columns: table => new
                {
                    TodoListID = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoList", x => x.TodoListID);
                });

            migrationBuilder.CreateTable(
                name: "TypesUserPermission",
                columns: table => new
                {
                    UserPermissionID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Category = table.Column<string>(type: "text", nullable: true)
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
                    Name = table.Column<string>(type: "text", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: true)
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
                    Login = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true)
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
                name: "TodoItem",
                columns: table => new
                {
                    TodoItemID = table.Column<Guid>(type: "uuid", nullable: false),
                    TodoListID = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    IsDone = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoItem", x => new { x.TodoItemID, x.TodoListID });
                    table.ForeignKey(
                        name: "FK_TodoItem_TodoList_TodoListID",
                        column: x => x.TodoListID,
                        principalTable: "TodoList",
                        principalColumn: "TodoListID",
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

            migrationBuilder.CreateIndex(
                name: "IX_TodoItem_TodoListID",
                table: "TodoItem",
                column: "TodoListID");

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
                name: "AuthToken");

            migrationBuilder.DropTable(
                name: "TodoItem");

            migrationBuilder.DropTable(
                name: "TypesUserRolePermission");

            migrationBuilder.DropTable(
                name: "UsersRoles");

            migrationBuilder.DropTable(
                name: "TodoList");

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
