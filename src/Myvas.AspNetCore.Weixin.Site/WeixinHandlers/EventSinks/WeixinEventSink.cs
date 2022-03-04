using Microsoft.Extensions.Logging;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore;
using Myvas.AspNetCore.Weixin.Models;
using System;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public class WeixinEventSink : IWeixinEventSink
{
    protected readonly ILogger _logger;
    protected readonly IWeixinResponseBuilder WeixinResponseBuilder;
    protected readonly IReceivedEntryStore<EventReceivedEntry> _eventStore;
    protected readonly IReceivedEntryStore<MessageReceivedEntry> _messageStore;

    public WeixinEventSink(ILogger<IWeixinEventSink> logger,
        IWeixinResponseBuilder responseBuilder,
        IReceivedEntryStore<EventReceivedEntry> eventStore,
        IReceivedEntryStore<MessageReceivedEntry> messageStore)
    {
        WeixinResponseBuilder = responseBuilder ?? throw new ArgumentNullException(nameof(responseBuilder));
        _eventStore = eventStore;// ?? throw new ArgumentNullException(nameof(eventStore));
        _messageStore = messageStore;// ?? throw new ArgumentNullException(nameof(messageStore));
        _logger = logger;
    }

    #region Messages
    public virtual async Task<bool> OnTextMessageReceived(WeixinEventContext<TextMessageReceivedXml> context)
    {
        _logger.LogTrace("OnTextMessageReceived: {content}", context.Xml.Content);

        // Store
        try
        {
            await _messageStore.StoreAsync<TextMessageReceivedEntry>(context.Xml.ToEntity());
            _logger.LogTrace("已将微信文本消息存入数据库。{content}", context.Xml.Content);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("将微信文本消息存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }

        return false;
    }

    public virtual async Task<bool> OnLinkMessageReceived(WeixinEventContext<LinkMessageReceivedXml> context)
    {
        _logger.LogTrace("OnLinkMessageReceived: {title}, {url}", context.Xml.Title, context.Xml.Url);

        // Store
        try
        {
            await _messageStore.StoreAsync(context.Xml.ToEntity());
            _logger.LogTrace("已将微信链接消息存入数据库。{title}, {url}", context.Xml.Title, context.Xml.Url);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("将微信链接消息存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }

        return false;
    }

    public virtual async Task<bool> OnVideoMessageReceived(WeixinEventContext<VideoMessageReceivedXml> context)
    {
        _logger.LogTrace("OnVideoMessageReceived: {mediaId}, {thumbMediaId}", context.Xml.MediaId, context.Xml.ThumbMediaId);

        // Store
        try
        {
            await _messageStore.StoreAsync(context.Xml.ToEntity());
            _logger.LogTrace("已将微信视频消息存入数据库。{mediaId}, {thumbMediaId}", context.Xml.MediaId, context.Xml.ThumbMediaId);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("将微信视频消息存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }

        return false;
    }

    public virtual async Task<bool> OnShortVideoMessageReceived(WeixinEventContext<ShortVideoMessageReceivedXml> context)
    {
        _logger.LogTrace("OnShortVideoMessageReceived: {mediaId}, {thumbMediaId}", context.Xml.MediaId, context.Xml.ThumbMediaId);

        // Store
        try
        {
            await _messageStore.StoreAsync(context.Xml.ToEntity());
            _logger.LogTrace("已将微信短视频消息存入数据库。{mediaId}, {thumbMediaId}", context.Xml.MediaId, context.Xml.ThumbMediaId);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("将微信短视频消息存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }

        return false;
    }

    public virtual async Task<bool> OnVoiceMessageReceived(WeixinEventContext<VoiceMessageReceivedXml> context)
    {
        _logger.LogTrace("OnVoiceMessageReceived: {format}, {mediaId}, {recognition}", context.Xml.Format, context.Xml.MediaId, context.Xml.Recognition);

        // Store
        try
        {
            await _messageStore.StoreAsync(context.Xml.ToEntity());
            _logger.LogTrace("已将微信语音消息存入数据库。{format}, {mediaId}, {recognition}", context.Xml.Format, context.Xml.MediaId, context.Xml.Recognition);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("将微信语音消息存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }

        return false;
    }

    public virtual async Task<bool> OnImageMessageReceived(WeixinEventContext<ImageMessageReceivedXml> context)
    {
        _logger.LogTrace("OnImageMessageReceived: {mediaId}, {picUrl}", context.Xml.MediaId, context.Xml.PicUrl);

        // Store
        try
        {
            await _messageStore.StoreAsync<ImageMessageReceivedEntry>(context.Xml.ToEntity());
            _logger.LogTrace("已将微信图片消息存入数据库。{mediaId}, {picUrl}", context.Xml.MediaId, context.Xml.PicUrl);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("将微信图片消息存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }

        return false;
    }

    public virtual async Task<bool> OnLocationMessageReceived(WeixinEventContext<LocationMessageReceivedXml> context)
    {
        _logger.LogTrace("OnLocationMessageReceived: {longitude}, {latitude}, {label}", context.Xml.Longitude, context.Xml.Latitude, context.Xml.Label);

        // Store
        try
        {
            await _messageStore.StoreAsync(context.Xml.ToEntity());
            _logger.LogTrace("已将微信位置消息存入数据库。{longitude}, {latitude}, {label}", context.Xml.Longitude, context.Xml.Latitude, context.Xml.Label);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("将微信位置消息存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }

        return false;
    }
    #endregion
    #region Events
    public virtual async Task<bool> OnLocationEventReceived(WeixinEventContext<LocationEventReceivedXml> context)
    {
        _logger.LogTrace("OnLocationEventReceived: {longitude}, {latitude}, {label}", context.Xml.Longitude, context.Xml.Latitude, context.Xml.FromUserName);

        // Store
        try
        {
            await _eventStore.StoreAsync(context.Xml.ToEntity());
            _logger.LogTrace("已将微信定位事件存入数据库。{longitude}, {latitude}, {label}", context.Xml.Longitude, context.Xml.Latitude, context.Xml.FromUserName);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("将微信定位事件存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }

        return false;
    }

    public virtual async Task<bool> OnClickMenuEventReceived(WeixinEventContext<ClickMenuEventReceivedXml> context)
    {
        _logger.LogTrace("OnClickMenuEventReceived: {eventKey}", context.Xml.EventKey);

        // Store
        try
        {
            await _eventStore.StoreAsync(context.Xml.ToEntity());
            _logger.LogTrace("已将微信点击事件存入数据库。{eventKey}", context.Xml.EventKey);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("将微信点击事件存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }

        return false;
    }

    public virtual async Task<bool> OnViewMenuEventReceived(WeixinEventContext<ViewMenuEventReceivedXml> context)
    {
        _logger.LogTrace("OnViewMenuEventReceived: {eventKey}", context.Xml.EventKey);

        // Store
        try
        {
            await _eventStore.StoreAsync(context.Xml.ToEntity());
            _logger.LogTrace("已将微信跳转事件存入数据库。{eventKey}", context.Xml.EventKey);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("将微信跳转事件存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }

        return false;
    }

    public virtual async Task<bool> OnUnsubscribeEventReceived(WeixinEventContext<UnsubscribeEventReceivedXml> context)
    {
        _logger.LogTrace("OnUnsubscribeEventReceived: {subscriber}", context.Xml.FromUserName);

        // Store
        try
        {
            await _eventStore.StoreAsync(context.Xml.ToEntity());
            _logger.LogTrace("已将微信退订事件存入数据库。{subscriber}", context.Xml.FromUserName);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("将微信退订事件存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }

        return false;
    }

    public virtual async Task<bool> OnSubscribeEventReceived(WeixinEventContext<SubscribeEventReceivedXml> context)
    {
        _logger.LogTrace("OnSubscribeEventReceived: {subscriber}, {eventKey}", context.Xml.FromUserName, context.Xml.EventKey);

        // Store
        try
        {
            await _eventStore.StoreAsync(context.Xml.ToEntity());
            _logger.LogTrace("已将微信订阅事件存入数据库。{subscriber}, {eventKey}", context.Xml.FromUserName, context.Xml.EventKey);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("将微信订阅事件存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }

        return false;
    }

    public virtual async Task<bool> OnQrscanEventReceived(WeixinEventContext<QrscanEventReceivedXml> context)
    {
        _logger.LogTrace("OnQrscanEventReceived: {eventKey}", context.Xml.EventKey);

        // Store
        try
        {
            await _eventStore.StoreAsync(context.Xml.ToEntity());
            _logger.LogTrace("已将微信扫码事件存入数据库。{eventKey}", context.Xml.EventKey);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("将微信扫码事件存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }

        return false;
    }
    #endregion
}
