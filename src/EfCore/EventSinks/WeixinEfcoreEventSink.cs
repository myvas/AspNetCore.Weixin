using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore;
using Myvas.AspNetCore.Weixin.Models;
using System;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// Save to database when message or event received.
/// </summary>
public class WeixinEfcoreEventSink : WeixinDebugEventSink
{
    protected readonly IReceivedEntryStore<EventReceivedEntry> _eventStore;
    protected readonly IReceivedEntryStore<MessageReceivedEntry> _messageStore;

    public WeixinEfcoreEventSink(IOptions<WeixinSiteOptions> optionsAccessor, ILogger<WeixinEfcoreEventSink> logger, IReceivedEntryStore<MessageReceivedEntry> messageStore, IReceivedEntryStore<EventReceivedEntry> eventStore)
        : base(optionsAccessor, logger)
    {
        _messageStore = messageStore ?? throw new ArgumentNullException(nameof(messageStore));
        _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
    }

    /// <inheritdoc/>
    public override Task<bool> OnImageMessageReceived(object sender, WeixinEventArgs<ImageMessageReceivedXml> e)
    {// Store
        try
        {
            _messageStore.StoreAsync<ImageMessageReceivedEntry>(e.Xml.ToEntity());
            _logger.LogTrace("已将微信图片消息存入数据库。{mediaId}, {picUrl}", e.Xml.MediaId, e.Xml.PicUrl);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("将微信图片消息存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }

        return base.OnImageMessageReceived(sender, e);
    }

    /// <inheritdoc/>
    public override Task<bool> OnLinkMessageReceived(object sender, WeixinEventArgs<LinkMessageReceivedXml> e)
    {
        // Store
        try
        {
            _messageStore.StoreAsync(e.Xml.ToEntity());
            _logger.LogTrace("已将微信链接消息存入数据库。{title}, {url}", e.Xml.Title, e.Xml.Url);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("将微信链接消息存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }

        return base.OnLinkMessageReceived(sender, e);
    }

    /// <inheritdoc/>
    public override Task<bool> OnLocationMessageReceived(object sender, WeixinEventArgs<LocationMessageReceivedXml> e)
    {
        // Store
        try
        {
            _messageStore.StoreAsync(e.Xml.ToEntity());
            _logger.LogTrace("已将微信位置消息存入数据库。{longitude}, {latitude}, {label}", e.Xml.Longitude, e.Xml.Latitude, e.Xml.Label);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("将微信位置消息存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }

        return base.OnLocationMessageReceived(sender, e);
    }

    /// <inheritdoc/>
    public override Task<bool> OnShortVideoMessageReceived(object sender, WeixinEventArgs<ShortVideoMessageReceivedXml> e)
    {
        // Store
        try
        {
            _messageStore.StoreAsync(e.Xml.ToEntity());
            _logger.LogTrace("已将微信短视频消息存入数据库。{mediaId}, {thumbMediaId}", e.Xml.MediaId, e.Xml.ThumbMediaId);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("将微信短视频消息存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }

        return base.OnShortVideoMessageReceived(sender, e);
    }

    /// <inheritdoc/>
    public override Task<bool> OnTextMessageReceived(object sender, WeixinEventArgs<TextMessageReceivedXml> e)
    {
        // Store
        try
        {
            _messageStore.StoreAsync<TextMessageReceivedEntry>(e.Xml.ToEntity());
            _logger.LogTrace("已将微信文本消息存入数据库。{content}", e.Xml.Content);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("将微信文本消息存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }

        return base.OnTextMessageReceived(sender, e);
    }

    /// <inheritdoc/>
    public override Task<bool> OnVideoMessageReceived(object sender, WeixinEventArgs<VideoMessageReceivedXml> e)
    {
        // Store
        try
        {
            _messageStore.StoreAsync(e.Xml.ToEntity());
            _logger.LogTrace("已将微信视频消息存入数据库。{mediaId}, {thumbMediaId}", e.Xml.MediaId, e.Xml.ThumbMediaId);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("将微信视频消息存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }


        return base.OnVideoMessageReceived(sender, e);
    }

    /// <inheritdoc/>
    public override Task<bool> OnVoiceMessageReceived(object sender, WeixinEventArgs<VoiceMessageReceivedXml> e)
    {
        // Store
        try
        {
            _messageStore.StoreAsync(e.Xml.ToEntity());
            _logger.LogTrace("已将微信语音消息存入数据库。{format}, {mediaId}, {recognition}", e.Xml.Format, e.Xml.MediaId, e.Xml.Recognition);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("将微信语音消息存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }

        return base.OnVoiceMessageReceived(sender, e);
    }

    /// <inheritdoc/>
    public override Task<bool> OnClickMenuEventReceived(object sender, WeixinEventArgs<ClickMenuEventReceivedXml> e)
    {
        // Store
        try
        {
            _eventStore.StoreAsync(e.Xml.ToEntity());
            _logger.LogTrace("已将微信点击事件存入数据库。{eventKey}", e.Xml.EventKey);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("将微信点击事件存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }

        return base.OnClickMenuEventReceived(sender, e);
    }

    /// <inheritdoc/>
    public override Task<bool> OnEnterEventReceived(object sender, WeixinEventArgs<EnterEventReceivedXml> e)
    {
        base.OnEnterEventReceived(sender, e);
        return ResponseWithText(e.Context, "");
    }

    /// <inheritdoc/>
    public override Task<bool> OnLocationEventReceived(object sender, WeixinEventArgs<LocationEventReceivedXml> e)
    {
        // Store
        try
        {
            _eventStore.StoreAsync(e.Xml.ToEntity());
            _logger.LogTrace("已将微信定位事件存入数据库。{longitude}, {latitude}, {fromUserName}", e.Xml.Longitude, e.Xml.Latitude, e.Xml.FromUserName);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("将微信定位事件存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }

        return base.OnLocationEventReceived(sender, e);
    }

    /// <inheritdoc/>
    public override Task<bool> OnQrscanEventReceived(object sender, WeixinEventArgs<QrscanEventReceivedXml> e)
    {
        // Store
        try
        {
            _eventStore.StoreAsync(e.Xml.ToEntity());
            _logger.LogTrace("已将微信扫码事件存入数据库。{eventKey}", e.Xml.EventKey);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("将微信扫码事件存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }

        return base.OnQrscanEventReceived(sender, e);
    }

    /// <inheritdoc/>
    public override Task<bool> OnSubscribeEventReceived(object sender, WeixinEventArgs<SubscribeEventReceivedXml> e)
    {
        // Store
        try
        {
            _eventStore.StoreAsync(e.Xml.ToEntity());
            _logger.LogTrace("已将微信订阅事件存入数据库。{subscriber}, {eventKey}", e.Xml.FromUserName, e.Xml.EventKey);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("将微信订阅事件存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }
        return base.OnSubscribeEventReceived(sender, e);
    }

    /// <inheritdoc/>
    public override Task<bool> OnUnsubscribeEventReceived(object sender, WeixinEventArgs<UnsubscribeEventReceivedXml> e)
    {
        // Store
        try
        {
            _eventStore.StoreAsync(e.Xml.ToEntity());
            _logger.LogTrace("已将微信退订事件存入数据库。{subscriber}", e.Xml.FromUserName);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("将微信退订事件存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }

        return base.OnUnsubscribeEventReceived(sender, e);
    }

    /// <inheritdoc/>
    public override Task<bool> OnViewMenuEventReceived(object sender, WeixinEventArgs<ViewMenuEventReceivedXml> e)
    {
        // Store
        try
        {
            _eventStore.StoreAsync(e.Xml.ToEntity());
            _logger.LogTrace("已将微信跳转事件存入数据库。{eventKey}", e.Xml.EventKey);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("将微信跳转事件存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }

        return base.OnViewMenuEventReceived(sender, e);
    }
}
