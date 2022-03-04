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
                name: "AuditEntires",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    TableName = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    KeyValue = table.Column<string>(type: "TEXT", nullable: true),
                    OldValue = table.Column<string>(type: "TEXT", nullable: true),
                    NewValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditEntires", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeixinReceivedEvents",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Event = table.Column<string>(type: "TEXT", nullable: false),
                    EventKey = table.Column<string>(type: "TEXT", nullable: true),
                    Latitude = table.Column<decimal>(type: "TEXT", nullable: true),
                    Longitude = table.Column<decimal>(type: "TEXT", nullable: true),
                    Precision = table.Column<decimal>(type: "TEXT", nullable: true),
                    Ticket = table.Column<string>(type: "TEXT", nullable: true),
                    ToUserName = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    FromUserName = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    CreateTime = table.Column<long>(type: "INTEGER", nullable: false),
                    MsgType = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeixinReceivedEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeixinReceivedMessages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    MsgId = table.Column<long>(type: "INTEGER", nullable: false),
                    PicUrl = table.Column<string>(type: "TEXT", nullable: true),
                    MediaId = table.Column<string>(type: "TEXT", nullable: true),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Url = table.Column<string>(type: "TEXT", nullable: true),
                    Latitude = table.Column<decimal>(type: "TEXT", nullable: true),
                    Longitude = table.Column<decimal>(type: "TEXT", nullable: true),
                    Scale = table.Column<decimal>(type: "TEXT", nullable: true),
                    Label = table.Column<string>(type: "TEXT", nullable: true),
                    ThumbMediaId = table.Column<string>(type: "TEXT", nullable: true),
                    Content = table.Column<string>(type: "TEXT", nullable: true),
                    Format = table.Column<string>(type: "TEXT", nullable: true),
                    Recognition = table.Column<string>(type: "TEXT", nullable: true),
                    ToUserName = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    FromUserName = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    CreateTime = table.Column<long>(type: "INTEGER", nullable: false),
                    MsgType = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeixinReceivedMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeixinSubscribers",
                columns: table => new
                {
                    OpenId = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    AppId = table.Column<string>(type: "TEXT", nullable: true),
                    UnionId = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    Gender = table.Column<int>(type: "INTEGER", nullable: true),
                    Nickname = table.Column<string>(type: "TEXT", nullable: true),
                    Remark = table.Column<string>(type: "TEXT", nullable: true),
                    City = table.Column<string>(type: "TEXT", nullable: true),
                    Province = table.Column<string>(type: "TEXT", nullable: true),
                    Country = table.Column<string>(type: "TEXT", nullable: true),
                    Language = table.Column<string>(type: "TEXT", nullable: true),
                    AvatorImageUrl = table.Column<string>(type: "TEXT", nullable: true),
                    SubscribedTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Unsubscribed = table.Column<bool>(type: "INTEGER", nullable: false),
                    UnsubscribedTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: true),
                    MentorId = table.Column<string>(type: "TEXT", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeixinSubscribers", x => x.OpenId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditEntires");

            migrationBuilder.DropTable(
                name: "WeixinReceivedEvents");

            migrationBuilder.DropTable(
                name: "WeixinReceivedMessages");

            migrationBuilder.DropTable(
                name: "WeixinSubscribers");
        }
    }
}
