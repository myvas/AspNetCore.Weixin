using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Demo.Migrations.WeixinDb
{
    public partial class AddWeixinSubscribers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReceivedTextMessages",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    From = table.Column<string>(nullable: true),
                    To = table.Column<string>(nullable: true),
                    ReceivedTime = table.Column<DateTimeOffset>(nullable: false),
                    Content = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceivedTextMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subscribers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    OpenId = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    NickName = table.Column<string>(nullable: true),
                    AvatorImage = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscribers", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReceivedTextMessages");

            migrationBuilder.DropTable(
                name: "Subscribers");
        }
    }
}
