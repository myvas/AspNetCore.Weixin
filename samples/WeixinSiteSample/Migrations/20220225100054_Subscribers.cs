using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeixinSiteSample.Migrations
{
    public partial class Subscribers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PersistedTokens",
                table: "PersistedTokens");

            migrationBuilder.DropIndex(
                name: "IX_PersistedTokens_AppId_Type",
                table: "PersistedTokens");

            migrationBuilder.DropIndex(
                name: "IX_PersistedTokens_ConsumedDate",
                table: "PersistedTokens");

            migrationBuilder.DropIndex(
                name: "IX_PersistedTokens_ExpirationDate",
                table: "PersistedTokens");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PersistedTokens");

            migrationBuilder.DropColumn(
                name: "ConsumedDate",
                table: "PersistedTokens");

            migrationBuilder.DropColumn(
                name: "ExpirationDate",
                table: "PersistedTokens");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "PersistedTokens");

            migrationBuilder.RenameColumn(
                name: "Data",
                table: "PersistedTokens",
                newName: "AccessToken");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "PersistedTokens",
                newName: "ExpirationTime");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersistedTokens",
                table: "PersistedTokens",
                column: "AppId");

            migrationBuilder.CreateTable(
                name: "WeixinSubscriber",
                columns: table => new
                {
                    OpenId = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Id = table.Column<string>(type: "TEXT", nullable: true),
                    UnionId = table.Column<string>(type: "TEXT", nullable: true),
                    Gender = table.Column<int>(type: "INTEGER", nullable: false),
                    Nickname = table.Column<string>(type: "TEXT", nullable: true),
                    City = table.Column<string>(type: "TEXT", nullable: true),
                    Province = table.Column<string>(type: "TEXT", nullable: true),
                    Country = table.Column<string>(type: "TEXT", nullable: true),
                    Language = table.Column<string>(type: "TEXT", nullable: true),
                    AvatorImageUrl = table.Column<string>(type: "TEXT", nullable: true),
                    SubscribedTime = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    Unsubscribed = table.Column<bool>(type: "INTEGER", maxLength: 1000, nullable: false),
                    UnsubscribedTime = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: true),
                    MentorId = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeixinSubscriber", x => x.OpenId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersistedTokens_ExpirationTime",
                table: "PersistedTokens",
                column: "ExpirationTime");

            migrationBuilder.CreateIndex(
                name: "IX_WeixinSubscriber_OpenId",
                table: "WeixinSubscriber",
                column: "OpenId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeixinSubscriber");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PersistedTokens",
                table: "PersistedTokens");

            migrationBuilder.DropIndex(
                name: "IX_PersistedTokens_ExpirationTime",
                table: "PersistedTokens");

            migrationBuilder.RenameColumn(
                name: "ExpirationTime",
                table: "PersistedTokens",
                newName: "CreationDate");

            migrationBuilder.RenameColumn(
                name: "AccessToken",
                table: "PersistedTokens",
                newName: "Data");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "PersistedTokens",
                type: "TEXT",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ConsumedDate",
                table: "PersistedTokens",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationDate",
                table: "PersistedTokens",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "PersistedTokens",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersistedTokens",
                table: "PersistedTokens",
                column: "Id");

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
    }
}
