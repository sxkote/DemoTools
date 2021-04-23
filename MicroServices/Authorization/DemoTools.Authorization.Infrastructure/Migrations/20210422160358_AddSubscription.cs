using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DemoTools.Authorization.Infrastructure.Migrations
{
    public partial class AddSubscription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Subscription",
                columns: table => new
                {
                    SubscriptionID = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(100)", fixedLength: true, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscription", x => x.SubscriptionID);
                });

            migrationBuilder.InsertData(
                table: "Subscription",
                columns: new[] { "SubscriptionID", "Title" },
                values: new object[] { new Guid("831b3626-6359-4f56-b5c2-81d86efcdc55"), "Demo-Subscription" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subscription");
        }
    }
}
