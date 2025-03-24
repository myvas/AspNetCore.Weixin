using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Myvas.AspNetCore.Weixin.EfCore;

namespace Myvas.AspNetCore.Weixin;

public class WeixinEfCoreEventSink : WeixinEfCoreEventSink<WeixinSubscriberEntity>
{
    public WeixinEfCoreEventSink(IOptions<WeixinSiteOptions> optionsAccessor,
        ILogger<WeixinEfCoreEventSink<WeixinSubscriberEntity>> logger,
        IWeixinReceivedMessageStore<WeixinReceivedMessageEntity> messageStore,
        IWeixinReceivedEventStore<WeixinReceivedEventEntity> eventStore,
        IWeixinSubscriberStore<WeixinSubscriberEntity> subscriberStore)
        : base(optionsAccessor, logger, messageStore, eventStore, subscriberStore)
    {
    }
}

public class WeixinEfCoreEventSink<TWeixinSubscriberEntity> : WeixinEfCoreEventSink<TWeixinSubscriberEntity, string>
    where TWeixinSubscriberEntity : class, IWeixinSubscriberEntity<string>, new()
{
    public WeixinEfCoreEventSink(IOptions<WeixinSiteOptions> optionsAccessor,
        ILogger<WeixinEfCoreEventSink<TWeixinSubscriberEntity, string>> logger,
        IWeixinReceivedMessageStore<WeixinReceivedMessageEntity> messageStore,
        IWeixinReceivedEventStore<WeixinReceivedEventEntity> eventStore,
        IWeixinSubscriberStore<TWeixinSubscriberEntity, string> subscriberStore)
        : base(optionsAccessor, logger, messageStore, eventStore, subscriberStore)
    {
    }
}

public class WeixinEfCoreEventSink<TWeixinSubscriberEntity, TKey> : WeixinTraceEventSink
    where TWeixinSubscriberEntity : class, IWeixinSubscriberEntity<TKey>, new()
    where TKey : IEquatable<TKey>
{
    protected readonly IWeixinReceivedEventStore<WeixinReceivedEventEntity> _eventStore;

    protected readonly IWeixinReceivedMessageStore<WeixinReceivedMessageEntity> _messageStore;

    protected readonly IWeixinSubscriberStore<TWeixinSubscriberEntity, TKey> _subscriberStore;

    public WeixinEfCoreEventSink(IOptions<WeixinSiteOptions> optionsAccessor,
        ILogger<WeixinEfCoreEventSink<TWeixinSubscriberEntity, TKey>> logger,
        IWeixinReceivedMessageStore<WeixinReceivedMessageEntity> messageStore,
        IWeixinReceivedEventStore<WeixinReceivedEventEntity> eventStore,
        IWeixinSubscriberStore<TWeixinSubscriberEntity, TKey> subscriberStore)
        : base(optionsAccessor, logger)
    {
        _messageStore = messageStore ?? throw new ArgumentNullException(nameof(messageStore));
        _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
        _subscriberStore = subscriberStore ?? throw new ArgumentNullException(nameof(subscriberStore));
    }

    protected async Task ResponseWithText(WeixinContext context, ReceivedXml xml, string entityInfo)
    {
        var stackTrace = new StackTrace();
        var list = new List<StackFrame>();
        for (int i = 1; i < stackTrace.FrameCount; i++)
        {
            list.Add(stackTrace.GetFrame(i));
        }

        string text = "";
        var hashSet = new HashSet<string>()
        {
            "ResponseWithText", "MoveNext", "Start", "SetStateMachine", "PerformWaitCallback",
            "ExecuteWithThreadLocal", "RunFromThreadPoolDispatchLoop", "ExecuteTask",
            "ExecutionContext.Run", "Task.ExecuteEntry",
            "Task.RunContinuations", "Task.FinishContinuations", "Invoke", "InvokeMethod",
            "RunInternal", "ThreadHelper.ThreadStart", "Dispatch"
        };
        foreach (StackFrame item in list)
        {
            string text2 = item?.GetMethod()?.Name;
            if (!hashSet.Contains(text2))
            {
                text = text2;
                break;
            }
        }

        var weixinResponseText = new WeixinResponseText(text + ": " + entityInfo)
        {
            ToUserName = xml.FromUserName,
            FromUserName = xml.ToUserName,
            CreateTime = DateTime.Now
        };
        await new WeixinResponseXmlBuilder(context.Context)
        {
            Content = weixinResponseText.ToXml()
        }
        .FlushAsync();
    }

    public override async Task<bool> OnTextMessageReceived(object sender, WeixinEventArgs<TextMessageReceivedXml> e)
    {
        await base.OnTextMessageReceived(sender, e);

        try
        {
            await _messageStore.CreateAsync(e.Xml.ToMessage());
            _logger.LogTrace("已将微信文本消息存入数据库。{content}", e.Xml.Content);
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex);
            _logger.LogWarning("将微信文本消息存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }

        var echo = $"收到一条微信文本消息：{e.Xml.Content}";
        await ResponseWithText(e.Context, e.Xml, echo);
        return true;
    }

    public override async Task<bool> OnLinkMessageReceived(object sender, WeixinEventArgs<LinkMessageReceivedXml> e)
    {
        await base.OnLinkMessageReceived(sender, e);

        try
        {
            var weixinResult = await _messageStore.CreateAsync(e.Xml.ToMessage());
            if (weixinResult.Succeeded)
            {
                _logger.LogTrace("已将微信链接消息存入数据库。{title}, {url}", e.Xml.Title, e.Xml.Url);
            }
            else
            {
                _logger.LogError("将微信链接消息存入数据库时失败。{title}, {url}, {errors}", e.Xml.Title, e.Xml.Url, weixinResult.Errors.ToString());
            }
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex);
            _logger.LogWarning("将微信链接消息存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }

        var weixinResponseNews = new WeixinResponseNews()
        {
            ToUserName = e.Xml.FromUserName,
            FromUserName = e.Xml.ToUserName,
            CreateTime = DateTime.Now,
            Articles = new List<WeixinResponseNewsArticle>(){new WeixinResponseNewsArticle{
                Title = $"收到一条链接消息: {e.Xml.Title}",
                Description = e.Xml.Description,
                Url = e.Xml.Url
            }}
        };
        await new WeixinResponseXmlBuilder(e.Context.Context)
        {
            Content = weixinResponseNews.ToXml()
        }
        .FlushAsync();
        return true;
    }

    public override async Task<bool> OnVideoMessageReceived(object sender, WeixinEventArgs<VideoMessageReceivedXml> e)
    {
        await base.OnVideoMessageReceived(sender, e);

        try
        {
            await _messageStore.CreateAsync(e.Xml.ToMessage());
            _logger.LogTrace("已将微信视频消息存入数据库。{mediaId}, {thumbMediaId}", e.Xml.MediaId, e.Xml.ThumbMediaId);
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex);
            _logger.LogWarning("将微信视频消息存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }

        var echo = $"收到一条视频消息，ID： {e.Xml.MediaId}";
        await ResponseWithText(e.Context, e.Xml, echo);
        return true;
    }

    public override async Task<bool> OnShortVideoMessageReceived(object sender, WeixinEventArgs<ShortVideoMessageReceivedXml> e)
    {
        await base.OnShortVideoMessageReceived(sender, e);

        try
        {
            await _messageStore.CreateAsync(e.Xml.ToMessage());
            _logger.LogTrace("已将微信短视频消息存入数据库。{mediaId}, {thumbMediaId}", e.Xml.MediaId, e.Xml.ThumbMediaId);
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex);
            _logger.LogWarning("将微信短视频消息存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }


        var echo = $"收到一条短视频消息，ID： {e.Xml.MediaId}";
        await ResponseWithText(e.Context, e.Xml, echo);
        return true;
    }

    public override async Task<bool> OnVoiceMessageReceived(object sender, WeixinEventArgs<VoiceMessageReceivedXml> e)
    {
        await base.OnVoiceMessageReceived(sender, e);

        try
        {
            await _messageStore.CreateAsync(e.Xml.ToMessage());
            _logger.LogTrace("已将微信语音消息存入数据库。{format}, {mediaId}, {recognition}", e.Xml.Format, e.Xml.MediaId, e.Xml.Recognition);
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex);
            _logger.LogWarning("将微信语音消息存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }

        var echo = $"收到一条语音消息，ID： {e.Xml.MediaId}";
        await ResponseWithText(e.Context, e.Xml, echo);
        return true;
    }

    public override async Task<bool> OnImageMessageReceived(object sender, WeixinEventArgs<ImageMessageReceivedXml> e)
    {
        await base.OnImageMessageReceived(sender, e);

        try
        {
            await _messageStore.CreateAsync(e.Xml.ToMessage());
            _logger.LogTrace("已将微信图片消息存入数据库。{mediaId}, {picUrl}", e.Xml.MediaId, e.Xml.PicUrl);
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex);
            _logger.LogWarning("将微信图片消息存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }

        var weixinResponseNews = new WeixinResponseNews()
        {
            ToUserName = e.Xml.FromUserName,
            FromUserName = e.Xml.ToUserName,
            CreateTime = DateTime.Now,
            Articles = new List<WeixinResponseNewsArticle>(){new WeixinResponseNewsArticle{
                Title = "收到一条图片信息",
                Description = "您发送的图片将会显示在边上",
                PicUrl = e.Xml.PicUrl,
                Url = "http://demo.auth.myvas.com"
            }}
        };
        await new WeixinResponseXmlBuilder(e.Context.Context)
        {
            Content = weixinResponseNews.ToXml()
        }
        .FlushAsync();
        return true;
    }

    public override async Task<bool> OnLocationMessageReceived(object sender, WeixinEventArgs<LocationMessageReceivedXml> e)
    {
        await base.OnLocationMessageReceived(sender, e);

        try
        {
            await _messageStore.CreateAsync(e.Xml.ToMessage());
            _logger.LogTrace("已将微信位置消息存入数据库。{longitude}, {latitude}, {label}", e.Xml.Longitude, e.Xml.Latitude, e.Xml.Label);
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex);
            _logger.LogWarning("将微信位置消息存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }

        var markersList = new List<GoogleMapMarkers>
        {
            new()
            {
                Latitude = e.Xml.Latitude,
                Longitude = e.Xml.Longitude,
                Color = "red",
                Label = "S",
                Size = GoogleMapMarkerSize.Default,
            }
        };
        var mapSize = "480x600";
        var mapUrl = GoogleMapHelper.GetGoogleStaticMap(19 /*requestMessage.Scale*//*微信和GoogleMap的Scale不一致，这里建议使用固定值*/,
            markersList, mapSize);
        var weixinResponseNews = new WeixinResponseNews()
        {
            ToUserName = e.Xml.FromUserName,
            FromUserName = e.Xml.ToUserName,
            CreateTime = DateTime.Now,
            Articles = new List<WeixinResponseNewsArticle>(){new WeixinResponseNewsArticle{
            Title = "定位地点周边地图",
                Description = string.Format("您刚才发送了地理位置信息。Location_X：{0}，Location_Y：{1}，Scale：{2}，标签：{3}",
                    e.Xml.Latitude, e.Xml.Longitude,
                    e.Xml.Scale, e.Xml.Label),
                PicUrl = mapUrl,
                Url = mapUrl
            }}
        };
        await new WeixinResponseXmlBuilder(e.Context.Context)
        {
            Content = weixinResponseNews.ToXml()
        }
        .FlushAsync();
        return true;
    }

    public override async Task<bool> OnLocationEventReceived(object sender, WeixinEventArgs<LocationEventReceivedXml> e)
    {
        await base.OnLocationEventReceived(sender, e);

        try
        {
            await _eventStore.CreateAsync(e.Xml.ToEvent());
            _logger.LogTrace("已将微信定位事件存入数据库。{longitude}, {latitude}, {fromUserName}", e.Xml.Longitude, e.Xml.Latitude, e.Xml.FromUserName);
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex);
            _logger.LogWarning("将微信定位事件存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }

        var markersList = new List<GoogleMapMarkers>
        {
            new()
            {
                Latitude = e.Xml.Latitude,
                Longitude = e.Xml.Longitude,
                Color = "red",
                Label = "S",
                Size = GoogleMapMarkerSize.Default,
            }
        };
        var mapSize = "480x600";
        var mapUrl = GoogleMapHelper.GetGoogleStaticMap(19 /*requestMessage.Scale*//*微信和GoogleMap的Scale不一致，这里建议使用固定值*/,
            markersList, mapSize);
        var weixinResponseNews = new WeixinResponseNews()
        {
            ToUserName = e.Xml.FromUserName,
            FromUserName = e.Xml.ToUserName,
            CreateTime = DateTime.Now,
            Articles = new List<WeixinResponseNewsArticle>(){new WeixinResponseNewsArticle{
            Title = "定位地点周边地图",
                Description = string.Format("您刚才发送了地理位置信息。Location_X：{0}，Location_Y：{1}，Precision： {2}",
                    e.Xml.Latitude, e.Xml.Longitude,
                    e.Xml.Precision),
                PicUrl = mapUrl,
                Url = mapUrl
            }}
        };
        await new WeixinResponseXmlBuilder(e.Context.Context)
        {
            Content = weixinResponseNews.ToXml()
        }
        .FlushAsync();
        return true;
    }

    public override async Task<bool> OnClickMenuEventReceived(object sender, WeixinEventArgs<ClickMenuEventReceivedXml> e)
    {
        await base.OnClickMenuEventReceived(sender, e);

        try
        {
            await _eventStore.CreateAsync(e.Xml.ToEvent());
            _logger.LogTrace("已将微信点击事件存入数据库。{eventKey}", e.Xml.EventKey);
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex);
            _logger.LogWarning("将微信点击事件存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }

        var echo = $"点击了子菜单按钮({e.Xml.FromUserName}): {e.Xml.EventKey}";
        await ResponseWithText(e.Context, e.Xml, echo);
        return true;
    }

    public override async Task<bool> OnViewMenuEventReceived(object sender, WeixinEventArgs<ViewMenuEventReceivedXml> e)
    {
        await base.OnViewMenuEventReceived(sender, e);

        try
        {
            await _eventStore.CreateAsync(e.Xml.ToEvent());
            _logger.LogTrace("已将微信跳转事件存入数据库。{eventKey}", e.Xml.EventKey);
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex);
            _logger.LogWarning("将微信跳转事件存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }

        var echo = $"点击了子菜单按钮({e.Xml.FromUserName}): {e.Xml.EventKeyAsUrl()}";
        await ResponseWithText(e.Context, e.Xml, echo);
        return true;
    }

    public override async Task<bool> OnUnsubscribeEventReceived(object sender, WeixinEventArgs<UnsubscribeEventReceivedXml> e)
    {
        await base.OnUnsubscribeEventReceived(sender, e);

        try
        {
            await _eventStore.CreateAsync(e.Xml.ToEvent());
            _logger.LogTrace("已将微信退订事件存入数据库。{subscriber}", e.Xml.FromUserName);
            var subscriber = await _subscriberStore.FindByOpenIdAsync(e.Xml.FromUserName);
            if (subscriber == null)
            {
                _logger.LogWarning("在微信订阅者数据中未找到记录。{subscriber}", e.Xml.FromUserName);
            }
            else
            {
                subscriber.Subscribed = false;
                subscriber.UnsubscribeTime = DateTimeOffset.Now.ToUnixTime();
                await _subscriberStore.UpdateAsync(subscriber);
                _logger.LogInformation("已在微信订阅者数据中标记退订。{subscriber}", e.Xml.FromUserName);
            }
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex);
            _logger.LogWarning("将微信退订事件存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }

        var echo = $"Unsubscribe({e.Xml.FromUserName})";
        await ResponseWithText(e.Context, e.Xml, echo);
        return true;
    }

    public override async Task<bool> OnQrscanEventReceived(object sender, WeixinEventArgs<QrscanEventReceivedXml> e)
    {
        await base.OnQrscanEventReceived(sender, e);

        try
        {
            await _eventStore.CreateAsync(e.Xml.ToEvent());
            _logger.LogTrace("已将微信扫码事件存入数据库。{eventKey}", e.Xml.EventKey);
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex);
            _logger.LogWarning("将微信扫码事件存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }

        var echo = $"QrscanEvent({e.Xml.FromUserName}: {e.Xml.EventKeyAsScene()}, {e.Xml.Ticket})";
        await ResponseWithText(e.Context, e.Xml, echo);
        return true;
    }

    public override async Task<bool> OnSubscribeEventReceived(object sender, WeixinEventArgs<SubscribeEventReceivedXml> e)
    {
        await base.OnSubscribeEventReceived(sender, e);

        try
        {
            await _eventStore.CreateAsync(e.Xml.ToEvent());
            _logger.LogTrace("已将微信订阅事件存入数据库。{subscriber}, {eventKey}", e.Xml.FromUserName, e.Xml.EventKey);
            var subscriber = await _subscriberStore.FindByOpenIdAsync(e.Xml.FromUserName);
            if (subscriber == null)
            {
                subscriber = new TWeixinSubscriberEntity
                {
                    OpenId = e.Xml.FromUserName,
                    SubscribeTime = DateTimeOffset.Now.ToUnixTime(),
                    Subscribed = true
                };
                await _subscriberStore.CreateAsync(subscriber);
                _logger.LogTrace("已将新的微信订阅者存入数据库。{subscriber}, {eventKey}", e.Xml.FromUserName, e.Xml.EventKey);
            }
            else
            {
                subscriber.SubscribeTime = DateTimeOffset.Now.ToUnixTime();
                subscriber.Subscribed = true;
                await _subscriberStore.UpdateAsync(subscriber);
                _logger.LogTrace("已将现有微信订阅者更新订阅标记。{subscriber}, {eventKey}", e.Xml.FromUserName, e.Xml.EventKey);
            }
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex);
            _logger.LogWarning("将微信订阅事件存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }

        var echo = $"Subscribe({e.Xml.FromUserName}: {e.Xml.EventKeyAsScene()}, {e.Xml.Ticket})";
        await ResponseWithText(e.Context, e.Xml, echo);
        return true;
    }

    public override async Task<bool> OnEnterEventReceived(object sender, WeixinEventArgs<EnterEventReceivedXml> e)
    {
        await base.OnEnterEventReceived(sender, e);

        try
        {
            await _eventStore.CreateAsync(e.Xml.ToEvent());
            _logger.LogTrace("已将微信点击事件存入数据库。");
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex);
            _logger.LogWarning("将微信点击事件存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }

        var echo = $"Enter({e.Xml.FromUserName})";
        await ResponseWithText(e.Context, e.Xml, echo);
        return true;
    }
}