using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Myvas.AspNetCore.Weixin;

namespace WeixinSiteSample.Data;

public class AppDbContext : IdentityDbContext, IWeixinDbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected AppDbContext()
    {
    }

    public DbSet<WeixinSubscriberEntity> WeixinSubscribers { get; set; }
    public DbSet<WeixinReceivedEventEntity> WeixinReceivedEvents { get; set; }
    public DbSet<WeixinReceivedMessageEntity> WeixinReceivedMessages { get; set; }
    public DbSet<WeixinResponseMessageEntity> WeixinResponseMessages { get; set; }
    public DbSet<WeixinSendMessageEntity> WeixinSendMessages { get; set; }
}