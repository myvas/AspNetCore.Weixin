using Microsoft.EntityFrameworkCore;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.Options;
using Myvas.AspNetCore.Weixin.Models;

namespace Myvas.AspNetCore.Weixin.EntityFrameworkCore.Storage.Extensions;

/// <summary>
/// Extensions methods to define the database schema for the configuration and operational data stores.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    /// Configures the persisted token context.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    /// <param name="storeOptions">The store options.</param>
    public static void ConfigureWeixinDbContext<TSubscriber>(this ModelBuilder modelBuilder, WeixinStoreOptions storeOptions)
        where TSubscriber : Subscriber
    {
        modelBuilder.Entity<TSubscriber>(entity =>
        {
            entity.ToTable("WeixinSubscribers");
            entity.HasKey(x => x.OpenId);
            entity.Property(x => x.OpenId).HasMaxLength(32).ValueGeneratedNever();
            entity.Property(x => x.Unsubscribed).IsRequired();
            //entity.Property(x => x.RowVersion).IsRowVersion();
        });

        #region Table-per-hierarchy for received messages: text, image, voice, video, shortvideo, location, link.
        modelBuilder.Entity<MessageReceivedEntry>(entity =>
        {
            entity.ToTable("WeixinReceivedMessages");
            entity.HasDiscriminator<string>(x => x.MsgType)
                .HasValue<TextMessageReceivedEntry>(nameof(RequestMsgType.text))
                .HasValue<ImageMessageReceivedEntry>(nameof(RequestMsgType.image))
                .HasValue<VoiceMessageReceivedEntry>(nameof(RequestMsgType.voice))
                .HasValue<VideoMessageReceivedEntry>(nameof(RequestMsgType.video))
                .HasValue<ShortVideoMessageReceivedEntry>(nameof(RequestMsgType.shortvideo))
                .HasValue<LocationMessageReceivedEntry>(nameof(RequestMsgType.location))
                .HasValue<LinkMessageReceivedEntry>(nameof(RequestMsgType.link));
        });
        modelBuilder.Entity<ImageMessageReceivedEntry>(entity =>
        {
            entity.Property(x => x.MediaId).HasColumnName(nameof(ImageMessageReceivedEntry.MediaId));
        });
        modelBuilder.Entity<VoiceMessageReceivedEntry>(entity =>
        {
            entity.Property(x => x.MediaId).HasColumnName(nameof(VideoMessageReceivedEntry.MediaId));
        });
        modelBuilder.Entity<ShortVideoMessageReceivedEntry>(entity =>
        {
            entity.Property(x => x.MediaId).HasColumnName(nameof(ShortVideoMessageReceivedEntry.MediaId));
            entity.Property(x => x.ThumbMediaId).HasColumnName(nameof(ShortVideoMessageReceivedEntry.ThumbMediaId));
        });
        modelBuilder.Entity<VideoMessageReceivedEntry>(entity =>
        {
            entity.Property(x => x.MediaId).HasColumnName(nameof(VideoMessageReceivedEntry.MediaId));
            entity.Property(x => x.ThumbMediaId).HasColumnName(nameof(VideoMessageReceivedEntry.ThumbMediaId));
        });
        #endregion

        #region Table-per-hierarchy for received events: subscribe, unsubscribe, enter, scan, click, view, location.
        modelBuilder.Entity<EventReceivedEntry>(entity =>
        {
            entity.ToTable("WeixinReceivedEvents");
            entity.HasDiscriminator<string>(x => x.Event)
                .HasValue<SubscribeEventReceivedEntry>(nameof(RequestEventType.subscribe))
                .HasValue<UnsubscribeEventReceivedEntry>(nameof(RequestEventType.unsubscribe))
                .HasValue<QrscanEventReceivedEntry>(nameof(RequestEventType.SCAN))
                .HasValue<ClickMenuEventReceivedEntry>(nameof(RequestEventType.CLICK))
                .HasValue<ViewMenuEventReceivedEntry>(nameof(RequestEventType.VIEW))
                .HasValue<LocationEventReceivedEntry>(nameof(RequestEventType.LOCATION));
        });
        modelBuilder.Entity<SubscribeEventReceivedEntry>(entity =>
        {
            entity.Property(x => x.EventKey).HasColumnName(nameof(SubscribeEventReceivedEntry.EventKey));
            entity.Property(x => x.Ticket).HasColumnName(nameof(SubscribeEventReceivedEntry.Ticket));
        });
        modelBuilder.Entity<QrscanEventReceivedEntry>(entity =>
        {
            entity.Property(x => x.EventKey).HasColumnName(nameof(QrscanEventReceivedEntry.EventKey));
            entity.Property(x => x.Ticket).HasColumnName(nameof(QrscanEventReceivedEntry.Ticket));
        });
        modelBuilder.Entity<ClickMenuEventReceivedEntry>(entity =>
        {
            entity.Property(x => x.EventKey).HasColumnName(nameof(ClickMenuEventReceivedEntry.EventKey));
        });
        modelBuilder.Entity<ViewMenuEventReceivedEntry>(entity =>
        {
            entity.Property(x => x.EventKey).HasColumnName(nameof(ViewMenuEventReceivedEntry.EventKey));
        });
        #endregion
    }
}
