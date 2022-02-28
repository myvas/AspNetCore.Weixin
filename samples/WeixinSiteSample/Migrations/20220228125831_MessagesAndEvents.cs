using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeixinSiteSample.Migrations
{
    public partial class MessagesAndEvents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersistedTokens");

            migrationBuilder.DropTable(
                name: "WeixinSubscriber");

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
                name: "ClickMenuReceivedEventEntries",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    EventKey = table.Column<string>(type: "TEXT", nullable: true),
                    ToUserName = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    FromUserName = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    CreateTime = table.Column<long>(type: "INTEGER", nullable: false),
                    MsgType = table.Column<string>(type: "TEXT", nullable: false),
                    Event = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClickMenuReceivedEventEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EnterReceivedEventEntries",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    ToUserName = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    FromUserName = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    CreateTime = table.Column<long>(type: "INTEGER", nullable: false),
                    MsgType = table.Column<string>(type: "TEXT", nullable: false),
                    Event = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnterReceivedEventEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LocationReceivedEventEntries",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Latitude = table.Column<decimal>(type: "TEXT", nullable: false),
                    Longitude = table.Column<decimal>(type: "TEXT", nullable: false),
                    Precision = table.Column<decimal>(type: "TEXT", nullable: false),
                    ToUserName = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    FromUserName = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    CreateTime = table.Column<long>(type: "INTEGER", nullable: false),
                    MsgType = table.Column<string>(type: "TEXT", nullable: false),
                    Event = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationReceivedEventEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MessageReceivedEntries",
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
                    table.PrimaryKey("PK_MessageReceivedEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QrscanReceivedEventEntries",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    EventKey = table.Column<string>(type: "TEXT", nullable: true),
                    Ticket = table.Column<string>(type: "TEXT", nullable: true),
                    ToUserName = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    FromUserName = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    CreateTime = table.Column<long>(type: "INTEGER", nullable: false),
                    MsgType = table.Column<string>(type: "TEXT", nullable: false),
                    Event = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QrscanReceivedEventEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubscribeReceivedEventEntries",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    EventKey = table.Column<string>(type: "TEXT", nullable: true),
                    Ticket = table.Column<string>(type: "TEXT", nullable: true),
                    ToUserName = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    FromUserName = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    CreateTime = table.Column<long>(type: "INTEGER", nullable: false),
                    MsgType = table.Column<string>(type: "TEXT", nullable: false),
                    Event = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscribeReceivedEventEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UnsubscribeReceivedEventEntries",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    ToUserName = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    FromUserName = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    CreateTime = table.Column<long>(type: "INTEGER", nullable: false),
                    MsgType = table.Column<string>(type: "TEXT", nullable: false),
                    Event = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnsubscribeReceivedEventEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ViewMenuReceivedEventEntries",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    EventKey = table.Column<string>(type: "TEXT", nullable: true),
                    ToUserName = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    FromUserName = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    CreateTime = table.Column<long>(type: "INTEGER", nullable: false),
                    MsgType = table.Column<string>(type: "TEXT", nullable: false),
                    Event = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViewMenuReceivedEventEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeixinSubscribers",
                columns: table => new
                {
                    OpenId = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Id = table.Column<string>(type: "TEXT", nullable: true),
                    UnionId = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    Gender = table.Column<int>(type: "INTEGER", nullable: true),
                    Nickname = table.Column<string>(type: "TEXT", nullable: true),
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
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
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
                name: "ClickMenuReceivedEventEntries");

            migrationBuilder.DropTable(
                name: "EnterReceivedEventEntries");

            migrationBuilder.DropTable(
                name: "LocationReceivedEventEntries");

            migrationBuilder.DropTable(
                name: "MessageReceivedEntries");

            migrationBuilder.DropTable(
                name: "QrscanReceivedEventEntries");

            migrationBuilder.DropTable(
                name: "SubscribeReceivedEventEntries");

            migrationBuilder.DropTable(
                name: "UnsubscribeReceivedEventEntries");

            migrationBuilder.DropTable(
                name: "ViewMenuReceivedEventEntries");

            migrationBuilder.DropTable(
                name: "WeixinSubscribers");

            migrationBuilder.CreateTable(
                name: "PersistedTokens",
                columns: table => new
                {
                    AppId = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    AccessToken = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    ExpirationTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersistedTokens", x => x.AppId);
                });

            migrationBuilder.CreateTable(
                name: "WeixinSubscriber",
                columns: table => new
                {
                    OpenId = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    AvatorImageUrl = table.Column<string>(type: "TEXT", nullable: true),
                    City = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    Country = table.Column<string>(type: "TEXT", nullable: true),
                    Gender = table.Column<int>(type: "INTEGER", nullable: false),
                    Id = table.Column<string>(type: "TEXT", nullable: true),
                    Language = table.Column<string>(type: "TEXT", nullable: true),
                    MentorId = table.Column<string>(type: "TEXT", nullable: true),
                    Nickname = table.Column<string>(type: "TEXT", nullable: true),
                    Province = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    SubscribedTime = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    UnionId = table.Column<string>(type: "TEXT", nullable: true),
                    Unsubscribed = table.Column<bool>(type: "INTEGER", maxLength: 1000, nullable: false),
                    UnsubscribedTime = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: true)
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
    }
}
