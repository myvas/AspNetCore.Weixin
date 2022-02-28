using Microsoft.EntityFrameworkCore;
using Myvas.AspNetCore.Weixin.Entities;

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
    /// Gets or sets the persisted tokens.
    /// </summary>
    /// <value>
    /// The persisted tokens.
    /// </value>
    DbSet<Entities.PersistedToken> PersistedTokens { get; set; }

    /// <summary>
    /// Gets or sets the weixin users.
    /// </summary>
    DbSet<Entities.WeixinSubscriber> WeixinSubscribers { get; set; }

    #region Received Messages
    DbSet<MessageReceivedEntity> ReceivedMessages { get; set; }
    //DbSet<TextMessageReceivedEntity> ReceivedTextMessages { get; set; }
    //DbSet<ImageMessageReceivedEntity> ReceivedImageMessages { get; set; }
    //DbSet<VoiceMessageReceivedEntity> ReceivedVoiceMessages { get; set; }
    //DbSet<VideoMessageReceivedEntity> ReceivedVideoMessages { get; set; }
    //DbSet<ShortVideoMessageReceivedEntity> ReceivedShortVideoMessages { get; set; }
    //DbSet<LocationMessageReceivedEntity> ReceivedLocationMessages { get; set; }
    //DbSet<LinkMessageReceivedEntity> ReceivedLinkMessages { get; set; }
    #endregion
    #region Received Events
    DbSet<SubscribeEventReceivedEntity> ReceivedSubscribeEvents { get; set; }
    DbSet<EnterEventReceivedEntity> ReceivedEnterEvents { get; set; }
    DbSet<ClickMenuEventReceivedEntity> ReceivedClickMenuEvents { get; set; }
    DbSet<ViewMenuEventReceivedEntity> ReceivedViewMenuEvents { get; set; }
    DbSet<QrscanEventReceivedEntity> ReceivedQrscanEvents { get; set; }
    DbSet<LocationEventReceivedEntity> ReceivedLocationEvents { get; set; }
    DbSet<UnsubscribeEventReceivedEntity> ReceivedUnsubscribeEvents { get; set; }
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