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
    public static void ConfigureWeixinDbContext(this ModelBuilder modelBuilder, WeixinStoreOptions storeOptions)
    {
        modelBuilder.Entity<WeixinSubscriber>(entity =>
        {
            //entity.ToTable("WeixinSubscribers");
            entity.HasKey(x => x.OpenId);
            entity.Property(x => x.OpenId).HasMaxLength(200).ValueGeneratedNever();
            entity.Property(x => x.Unsubscribed).IsRequired();
            entity.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        });

        //Table - per - hierarchy for received messages: text, image, voice, video, shortvideo, location, link.
        modelBuilder.Entity<MessageReceivedEntry>(entity =>
        {
            entity.HasDiscriminator<string>(x => x.MsgType)
                .HasValue<TextMessageReceivedEntry>("text")
                .HasValue<ImageMessageReceivedEntry>("image")
                .HasValue<VoiceMessageReceivedEntry>("voice")
                .HasValue<VideoMessageReceivedEntry>("video")
                .HasValue<ShortVideoMessageReceivedEntry>("shortvideo")
                .HasValue<LocationMessageReceivedEntry>("location")
                .HasValue<LinkMessageReceivedEntry>("link");
        });
        modelBuilder.Entity<ImageMessageReceivedEntry>(entity =>
        {
            entity.Property(x => x.MediaId).HasColumnName(nameof(ImageMessageReceivedEntry.MediaId));
        });
        modelBuilder.Entity<ShortVideoMessageReceivedEntry>(entity =>
        {
            entity.Property(x => x.MediaId).HasColumnName(nameof(ShortVideoMessageReceivedEntry.MediaId));
        });
        modelBuilder.Entity<ShortVideoMessageReceivedEntry>(entity =>
        {
            entity.Property(x => x.ThumbMediaId).HasColumnName(nameof(ShortVideoMessageReceivedEntry.ThumbMediaId));
        });
        modelBuilder.Entity<VideoMessageReceivedEntry>(entity =>
        {
            entity.Property(x => x.MediaId).HasColumnName(nameof(VideoMessageReceivedEntry.MediaId));
        });
        modelBuilder.Entity<VideoMessageReceivedEntry>(entity =>
        {
            entity.Property(x => x.ThumbMediaId).HasColumnName(nameof(VideoMessageReceivedEntry.ThumbMediaId));
        });
        modelBuilder.Entity<VoiceMessageReceivedEntry>(entity =>
        {
            entity.Property(x => x.MediaId).HasColumnName(nameof(VideoMessageReceivedEntry.MediaId));
        });
    }
}
