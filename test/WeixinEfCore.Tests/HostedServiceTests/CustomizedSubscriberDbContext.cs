using Microsoft.EntityFrameworkCore;

namespace Myvas.AspNetCore.Weixin.EfCore.Tests.HostedServiceTests;

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
