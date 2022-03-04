using Microsoft.EntityFrameworkCore;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.DbContexts;
using Myvas.AspNetCore.Weixin.Models;

namespace Myvas.AspNetCore.Weixin;

#pragma warning disable 1591

/// <summary>
/// Abstraction for the <see cref="WeixinDbContext{TSubscriber}"/>.
/// </summary>
/// <seealso cref="IDisposable"/>
public interface IWeixinDbContext<TSubscriber> : IDisposable
    where TSubscriber : Subscriber
{
    /// <summary>
    /// The <see cref="AuditEntry"/> entities.
    /// </summary>
    DbSet<AuditEntry> AuditEntires { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="Subscriber"/>.
    /// </summary>
    DbSet<TSubscriber> Subscribers { get; set; }

    /// <summary>
    /// Received messages, Table-per-hierarchy
    /// </summary>
    DbSet<MessageReceivedEntry> MessageReceivedEntries { get; set; }
    #region Received Messages, Table-per-type
    //DbSet<TextMessageReceivedEntry> TextMessageReceivedEntries { get; set; }
    //DbSet<ImageMessageReceivedEntry> ImageMessageReceivedEntries { get; set; }
    //DbSet<VoiceMessageReceivedEntry> VoiceMessageReceivedEntries { get; set; }
    //DbSet<VideoMessageReceivedEntry> VideoMessageReceivedEntries { get; set; }
    //DbSet<ShortVideoMessageReceivedEntry> ShortVideoMessageReceivedEntries { get; set; }
    //DbSet<LocationMessageReceivedEntry> LocationMessageReceivedEntries { get; set; }
    //DbSet<LinkMessageReceivedEntry> LinkMessageReceivedEntries { get; set; }
    #endregion

    /// <summary>
    /// Received events, Table-per-hierarchy
    /// </summary>
    DbSet<EventReceivedEntry> EventReceivedEntries { get; set; }
    #region Received Events: Table-per-type
    //DbSet<SubscribeEventReceivedEntry> SubscribeEventReceivedEntries { get; set; }
    //DbSet<EnterEventReceivedEntry> EnterEventReceivedEntries { get; set; }
    //DbSet<ClickMenuEventReceivedEntry> ClickMenuEventReceivedEntries { get; set; }
    //DbSet<ViewMenuEventReceivedEntry> ViewMenuEventReceivedEntries { get; set; }
    //DbSet<QrscanEventReceivedEntry> QrscanEventReceivedEntries { get; set; }
    //DbSet<LocationEventReceivedEntry> LocationEventReceivedEntries { get; set; }
    //DbSet<UnsubscribeEventReceivedEntry> UnsubscribeEventReceivedEntries { get; set; }
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