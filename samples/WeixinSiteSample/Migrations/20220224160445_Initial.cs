using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeixinSiteSample.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PersistedTokens",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    AppId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ConsumedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Data = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersistedTokens", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersistedTokens_AppId_Type",
                table: "PersistedTokens",
                columns: new[] { "AppId", "Type" });

            migrationBuilder.CreateIndex(
                name: "IX_PersistedTokens_ConsumedDate",
                table: "PersistedTokens",
                column: "ConsumedDate");

            migrationBuilder.CreateIndex(
                name: "IX_PersistedTokens_ExpirationDate",
                table: "PersistedTokens",
                column: "ExpirationDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersistedTokens");
        }
    }
}
