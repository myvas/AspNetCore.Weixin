using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin.EfCore.Tests;

public class MySubscriber : WeixinSubscriberEntity
{

}

public class CustomizedSubscriberDbContext : DbContext, IWeixinDbContext<MySubscriber, string>
{
    public CustomizedSubscriberDbContext(DbContextOptions<CustomizedSubscriberDbContext> options) : base(options)
    {
    }

    public DbSet<MySubscriber> WeixinSubscribers { get; set; }
    public DbSet<WeixinReceivedEventEntity> WeixinReceivedEvents { get; set; }
    public DbSet<WeixinReceivedMessageEntity> WeixinReceivedMessages { get; set; }
    public DbSet<WeixinResponseMessageEntity> WeixinResponseMessages { get; set; }
    public DbSet<WeixinSendMessageEntity> WeixinSendMessages { get; set; }
}

public class CustomizedSubscriberDbContextTests
{
    public const string MicroMessengerUserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 5_1 like Mac OS X) AppleWebKit/534.46 (KHTML, like Gecko) Mobile/9B176 MicroMessenger/4.3.2";

    private readonly TestServer testServer;

    public CustomizedSubscriberDbContextTests()
    {
        testServer = TestServerBuilder.CreateServer(app =>
        {
            app.UseWeixinSite();
        }, services =>
        {
            services.AddDbContext<CustomizedSubscriberDbContext>(o =>
            {
                o.UseInMemoryDatabase(databaseName: "DerivedWeixinTestDatabase" + Guid.NewGuid().ToString("N"));
            });

            services.AddWeixin(o =>
            {
                o.AppId = "APPID";
                o.AppSecret = "NOT_USED_APPSECRET";
            })
            .AddWeixinSite(o =>
            {
                o.WebsiteToken = "WEIXINSITETOKEN";
            })
            .AddMessageProtection()
            .AddWeixinEfCore<CustomizedSubscriberDbContext>();
        },
        async context =>
        {
            var req = context.Request;
            if (!req.Path.Value.Equals(WeixinSiteOptionsDefaults.Path))
            {

                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                var content = "404 NOT FOUND";
                await context.Response.WriteAsync(content);
                return true;
            }
            return false;
        });
    }

    [Theory]
    [InlineData("uplink/event/subscribe_qrscene.xml", "OnSubscribeEventReceived: EventKey: qrscene_123123, Ticket: TICKET", "from-event-subscribe-qrscene")]
    public async Task HttpPost_WeixinEvent_Subscribe(string fileName, string result, string fromUserName)
    {
        var testClient = testServer.CreateClient();
        var textXml = TestFile.ReadTestFile(fileName);
        var timestamp = DateTime.Now.Ticks.ToString();
        var nonce = "nonce";
        var signature = SignatureHelper.CalculateSignature(timestamp, nonce, "WEIXINSITETOKEN");
        var query = new QueryBuilder
        {
            { "signature", signature },
            { "timestamp", timestamp },
            { "nonce", nonce }
        };
        var uri = WeixinSiteOptionsDefaults.Path + query.ToString();
        testClient.DefaultRequestHeaders.Add("User-Agent", MicroMessengerUserAgent);

        var response = await testClient.PostAsync(uri, new StringContent(textXml));
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        // Assert message on client-side
        var s = await response.Content.ReadAsStringAsync();
        Assert.StartsWith("<xml>", s);
        Assert.Contains($"<Content>{result}</Content>", s);
        Assert.EndsWith("</xml>", s);

        // Assert db on server-side
        var db = testServer.Services.GetRequiredService<CustomizedSubscriberDbContext>();
        {
            var entity = await db.WeixinReceivedEvents.FirstOrDefaultAsync(x => x.FromUserName == fromUserName);
            Assert.NotNull(entity);

            var subscriber = await db.WeixinSubscribers.FirstOrDefaultAsync(x => x.OpenId == fromUserName);
            Assert.NotNull(entity);
        }
    }
}
