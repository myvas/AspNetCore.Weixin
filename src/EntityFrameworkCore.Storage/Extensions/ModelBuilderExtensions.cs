using Microsoft.EntityFrameworkCore;
using Myvas.AspNetCore.Weixin.Entities;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.Options;

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
    public static void ConfigureWeixinDbContext(this ModelBuilder modelBuilder, WeixinStoreOptions storeOptions)
    {
        modelBuilder.Entity<PersistedToken>(entity =>
        {
            entity.HasKey(x => x.AppId);
            entity.Property(x => x.AppId).HasMaxLength(200).ValueGeneratedNever();
            entity.Property(x => x.AccessToken).HasMaxLength(1000).IsRequired();
            entity.Property(x => x.ExpirationTime).IsRequired();
            entity.HasIndex(x => x.ExpirationTime);
        });

        modelBuilder.Entity<WeixinSubscriber>(entity =>
        {
            //entity.ToTable("WeixinSubscribers");
            entity.HasKey(x => x.OpenId);
            entity.Property(x => x.OpenId).HasMaxLength(200).ValueGeneratedNever();
            entity.Property(x => x.Unsubscribed).IsRequired();
            entity.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        });

        //Table - per - hierarchy for received messages: text, image, voice, video, shortvideo, location, link.
        modelBuilder.Entity<MessageReceivedEntity>(entity =>
        {
            entity.HasDiscriminator<string>(x => x.MsgType)
                .HasValue<TextMessageReceivedEntity>("text")
                .HasValue<ImageMessageReceivedEntity>("image")
                .HasValue<VoiceMessageReceivedEntity>("voice")
                .HasValue<VideoMessageReceivedEntity>("video")
                .HasValue<ShortVideoMessageReceivedEntity>("shortvideo")
                .HasValue<LocationMessageReceivedEntity>("location")
                .HasValue<LinkMessageReceivedEntity>("link");
        });
        modelBuilder.Entity<ImageMessageReceivedEntity>(entity =>
        {
            entity.Property(x => x.MediaId).HasColumnName(nameof(ImageMessageReceivedEntity.MediaId));
        });
        modelBuilder.Entity<ShortVideoMessageReceivedEntity>(entity =>
        {
            entity.Property(x => x.MediaId).HasColumnName(nameof(ShortVideoMessageReceivedEntity.MediaId));
        });
        modelBuilder.Entity<ShortVideoMessageReceivedEntity>(entity =>
        {
            entity.Property(x => x.ThumbMediaId).HasColumnName(nameof(ShortVideoMessageReceivedEntity.ThumbMediaId));
        });
        modelBuilder.Entity<VideoMessageReceivedEntity>(entity =>
        {
            entity.Property(x => x.MediaId).HasColumnName(nameof(VideoMessageReceivedEntity.MediaId));
        });
        modelBuilder.Entity<VideoMessageReceivedEntity>(entity =>
        {
            entity.Property(x => x.ThumbMediaId).HasColumnName(nameof(VideoMessageReceivedEntity.ThumbMediaId));
        });
        modelBuilder.Entity<VoiceMessageReceivedEntity>(entity =>
        {
            entity.Property(x => x.MediaId).HasColumnName(nameof(VideoMessageReceivedEntity.MediaId));
        });
    }
}
