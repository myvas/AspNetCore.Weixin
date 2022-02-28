using Microsoft.EntityFrameworkCore;
using Myvas.AspNetCore.Weixin.Models;

namespace Myvas.AspNetCore.Weixin;

#pragma warning disable 1591

/// <summary>
/// Abstraction for the operational data context.
/// </summary>
/// <seealso cref="IDisposable"/>
public interface IWeixinDbContext : IDisposable
{
    public DbSet<AuditEntry> AuditEntires { get; set; }

    /// <summary>
    /// Gets or sets the weixin users.
    /// </summary>
    DbSet<WeixinSubscriber> WeixinSubscribers { get; set; }

    #region Received Messages, Table-per-hierarchy
    DbSet<MessageReceivedEntry> MessageReceivedEntries { get; set; }
    //DbSet<TextMessageReceivedEntry> TextMessageReceivedEntries { get; set; }
    //DbSet<ImageMessageReceivedEntry> ImageMessageReceivedEntries { get; set; }
    //DbSet<VoiceMessageReceivedEntry> VoiceMessageReceivedEntries { get; set; }
    //DbSet<VideoMessageReceivedEntry> VideoMessageReceivedEntries { get; set; }
    //DbSet<ShortVideoMessageReceivedEntry> ShortVideoMessageReceivedEntries { get; set; }
    //DbSet<LocationMessageReceivedEntry> LocationMessageReceivedEntries { get; set; }
    //DbSet<LinkMessageReceivedEntry> LinkMessageReceivedEntries { get; set; }
    #endregion
    #region Received Events: Table-per-type
    DbSet<SubscribeEventReceivedEntry> SubscribeReceivedEventEntries { get; set; }
    DbSet<EnterEventReceivedEntry> EnterReceivedEventEntries { get; set; }
    DbSet<ClickMenuEventReceivedEntry> ClickMenuReceivedEventEntries { get; set; }
    DbSet<ViewMenuEventReceivedEntry> ViewMenuReceivedEventEntries { get; set; }
    DbSet<QrscanEventReceivedEntry> QrscanReceivedEventEntries { get; set; }
    DbSet<LocationEventReceivedEntry> LocationReceivedEventEntries { get; set; }
    DbSet<UnsubscribeEventReceivedEntry> UnsubscribeReceivedEventEntries { get; set; }
    #endregion

    /// <summary>
    /// Saves the changes.
    /// </summary>
    /// <returns></returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Saves the changes.
    /// </summary>
    /// <returns></returns>
    Task<int> SaveChangesAsync() => SaveChangesAsync(CancellationToken.None);
}