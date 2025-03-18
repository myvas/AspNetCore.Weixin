using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Myvas.AspNetCore.Weixin.EfCore;
using Myvas.AspNetCore.Weixin.Models;
using System;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// Save to database when message or event received.
/// </summary>
public class WeixinEfCoreEventSink : WeixinDebugEventSink
{
    protected readonly IWeixinReceivedEventStore<WeixinReceivedEvent> _eventStore;
    protected readonly IWeixinReceivedMessageStore<WeixinReceivedMessage> _messageStore;

    public WeixinEfCoreEventSink(IOptions<WeixinSiteOptions> optionsAccessor, ILogger<WeixinEfCoreEventSink> logger,
        IWeixinReceivedMessageStore<WeixinReceivedMessage> messageStore, IWeixinReceivedEventStore<WeixinReceivedEvent> eventStore)
        : base(optionsAccessor, logger)
    {
        _messageStore = messageStore ?? throw new ArgumentNullException(nameof(messageStore));
        _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
    }

    /// <inheritdoc/>
    public override async Task<bool> OnImageMessageReceived(object sender, WeixinEventArgs<ImageMessageReceivedXml> e)
    {// Store
        try
        {
            await _messageStore.CreateAsync(e.Xml.ToMessage());
            _logger.LogTrace("已将微信图片消息存入数据库。{mediaId}, {picUrl}", e.Xml.MediaId, e.Xml.PicUrl);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("将微信图片消息存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }

        return await base.OnImageMessageReceived(sender, e);
    }

    /// <inheritdoc/>
    public override async Task<bool> OnLinkMessageReceived(object sender, WeixinEventArgs<LinkMessageReceivedXml> e)
    {
        // Store
        try
        {
            var result = await _messageStore.CreateAsync(e.Xml.ToMessage());
            if (result.Succeeded)
                _logger.LogTrace("已将微信链接消息存入数据库。{title}, {url}", e.Xml.Title, e.Xml.Url);
            else
                _logger.LogError("将微信链接消息存入数据库时失败。{title}, {url}, {errors}", e.Xml.Title, e.Xml.Url, result.Errors.ToString());

        }
        catch (Exception ex)
        {
            _logger.LogWarning("将微信链接消息存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }

        return await base.OnLinkMessageReceived(sender, e);
    }

    /// <inheritdoc/>
    public override async Task<bool> OnLocationMessageReceived(object sender, WeixinEventArgs<LocationMessageReceivedXml> e)
    {
        // Store
        try
        {
            await _messageStore.CreateAsync(e.Xml.ToMessage());
            _logger.LogTrace("已将微信位置消息存入数据库。{longitude}, {latitude}, {label}", e.Xml.Longitude, e.Xml.Latitude, e.Xml.Label);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("将微信位置消息存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }

        return await base.OnLocationMessageReceived(sender, e);
    }

    /// <inheritdoc/>
    public override async Task<bool> OnShortVideoMessageReceived(object sender, WeixinEventArgs<ShortVideoMessageReceivedXml> e)
    {
        // Store
        try
        {
            await _messageStore.CreateAsync(e.Xml.ToMessage());
            _logger.LogTrace("已将微信短视频消息存入数据库。{mediaId}, {thumbMediaId}", e.Xml.MediaId, e.Xml.ThumbMediaId);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("将微信短视频消息存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }

        return await base.OnShortVideoMessageReceived(sender, e);
    }

    /// <inheritdoc/>
    public override async Task<bool> OnTextMessageReceived(object sender, WeixinEventArgs<TextMessageReceivedXml> e)
    {
        // Store
        try
        {
            await _messageStore.CreateAsync(e.Xml.ToMessage());
            _logger.LogTrace("已将微信文本消息存入数据库。{content}", e.Xml.Content);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("将微信文本消息存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }

        return await base.OnTextMessageReceived(sender, e);
    }

    /// <inheritdoc/>
    public override async Task<bool> OnVideoMessageReceived(object sender, WeixinEventArgs<VideoMessageReceivedXml> e)
    {
        // Store
        try
        {
            await _messageStore.CreateAsync(e.Xml.ToMessage());
            _logger.LogTrace("已将微信视频消息存入数据库。{mediaId}, {thumbMediaId}", e.Xml.MediaId, e.Xml.ThumbMediaId);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("将微信视频消息存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }


        return await base.OnVideoMessageReceived(sender, e);
    }

    /// <inheritdoc/>
    public override async Task<bool> OnVoiceMessageReceived(object sender, WeixinEventArgs<VoiceMessageReceivedXml> e)
    {
        // Store
        try
        {
            await _messageStore.CreateAsync(e.Xml.ToMessage());
            _logger.LogTrace("已将微信语音消息存入数据库。{format}, {mediaId}, {recognition}", e.Xml.Format, e.Xml.MediaId, e.Xml.Recognition);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("将微信语音消息存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }

        return await base.OnVoiceMessageReceived(sender, e);
    }

    /// <inheritdoc/>
    public override async Task<bool> OnClickMenuEventReceived(object sender, WeixinEventArgs<ClickMenuEventReceivedXml> e)
    {
        // Store
        try
        {
            await _eventStore.CreateAsync(e.Xml.ToEvent());
            _logger.LogTrace("已将微信点击事件存入数据库。{eventKey}", e.Xml.EventKey);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("将微信点击事件存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }

        return await base.OnClickMenuEventReceived(sender, e);
    }

    /// <inheritdoc/>
    public override async Task<bool> OnEnterEventReceived(object sender, WeixinEventArgs<EnterEventReceivedXml> e)
    {
        // Store
        try
        {
            await _eventStore.CreateAsync(e.Xml.ToEvent());
            _logger.LogTrace("已将微信点击事件存入数据库。");
        }
        catch (Exception ex)
        {
            _logger.LogWarning("将微信点击事件存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }

        return await base.OnEnterEventReceived(sender, e);
    }

    /// <inheritdoc/>
    public override async Task<bool> OnLocationEventReceived(object sender, WeixinEventArgs<LocationEventReceivedXml> e)
    {
        // Store
        try
        {
            await _eventStore.CreateAsync(e.Xml.ToEvent());
            _logger.LogTrace("已将微信定位事件存入数据库。{longitude}, {latitude}, {fromUserName}", e.Xml.Longitude, e.Xml.Latitude, e.Xml.FromUserName);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("将微信定位事件存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }

        return await base.OnLocationEventReceived(sender, e);
    }

    /// <inheritdoc/>
    public override async Task<bool> OnQrscanEventReceived(object sender, WeixinEventArgs<QrscanEventReceivedXml> e)
    {
        // Store
        try
        {
            await _eventStore.CreateAsync(e.Xml.ToEvent());
            _logger.LogTrace("已将微信扫码事件存入数据库。{eventKey}", e.Xml.EventKey);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("将微信扫码事件存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }

        return await base.OnQrscanEventReceived(sender, e);
    }

    /// <inheritdoc/>
    public override async Task<bool> OnSubscribeEventReceived(object sender, WeixinEventArgs<SubscribeEventReceivedXml> e)
    {
        // Store
        try
        {
            await _eventStore.CreateAsync(e.Xml.ToEvent());
            _logger.LogTrace("已将微信订阅事件存入数据库。{subscriber}, {eventKey}", e.Xml.FromUserName, e.Xml.EventKey);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("将微信订阅事件存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }
        return await base.OnSubscribeEventReceived(sender, e);
    }

    /// <inheritdoc/>
    public override async Task<bool> OnUnsubscribeEventReceived(object sender, WeixinEventArgs<UnsubscribeEventReceivedXml> e)
    {
        // Store
        try
        {
            await _eventStore.CreateAsync(e.Xml.ToEvent());
            _logger.LogTrace("已将微信退订事件存入数据库。{subscriber}", e.Xml.FromUserName);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("将微信退订事件存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }

        return await base.OnUnsubscribeEventReceived(sender, e);
    }

    /// <inheritdoc/>
    public override async Task<bool> OnViewMenuEventReceived(object sender, WeixinEventArgs<ViewMenuEventReceivedXml> e)
    {
        // Store
        try
        {
            await _eventStore.CreateAsync(e.Xml.ToEvent());
            _logger.LogTrace("已将微信跳转事件存入数据库。{eventKey}", e.Xml.EventKey);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("将微信跳转事件存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }

        return await base.OnViewMenuEventReceived(sender, e);
    }
}
