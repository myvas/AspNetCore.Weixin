using Myvas.AspNetCore.Weixin;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore;
using Myvas.AspNetCore.Weixin.Models;
using System.Text;

namespace WeixinSiteSample;

public class DefaultWeixinEventSink : WeixinEventSink
{
    private readonly WeixinSubscriberManager<ApplicationUser> _subscriberManager;

    public DefaultWeixinEventSink(ILogger<IWeixinEventSink> logger,
        IWeixinResponseBuilder responseBuilder,
        IReceivedEntryStore<EventReceivedEntry> eventStore,
        IReceivedEntryStore<MessageReceivedEntry> messageStore,
        WeixinSubscriberManager<ApplicationUser> subscriberManager) : base(logger, responseBuilder, eventStore, messageStore)
    {
        _subscriberManager = subscriberManager ?? throw new ArgumentNullException(nameof(subscriberManager));
    }

    public override async Task<bool> OnSubscribeEventReceived(WeixinEventContext<SubscribeEventReceivedXml> context)
    {
        await base.OnSubscribeEventReceived(context);

        // Adds a new entity or updates a existing entity in the db.
        var result = await _subscriberManager.SubscribeAsync(context.Xml.ToEntity());
        if (result)
        {
            _logger.LogDebug("Added a new subscriber or updated a existing subscriber in the db: {openId}", context.Xml.FromUserName);
        }
        else
        {
            _logger.LogWarning("Failed to fetch the subscriber's profile, nor persist it into the db: {openId}", context.Xml.FromUserName);
        }

        return true;
    }

    public override async Task<bool> OnTextMessageReceived(WeixinEventContext<TextMessageReceivedXml> context)
    {
        await base.OnTextMessageReceived(context);

        var result = new StringBuilder();
        result.AppendFormat("收到一条微信文本消息：{0}", context.Xml.Content);
        await WeixinResponseBuilder.FlushTextMessage(context.Context, context.Xml, result.ToString());
        _logger.LogDebug("FlushTextMessage: {content}", result.ToString());

        return true;
    }

    public override async Task<bool> OnImageMessageReceived(WeixinEventContext<ImageMessageReceivedXml> context)
    {
        await base.OnImageMessageReceived(context);

        var result = new StringBuilder();
        result.AppendFormat("收到一条微信图片消息：{0}, {1}", context.Xml.MediaId, context.Xml.PicUrl);

        await WeixinResponseBuilder.FlushTextMessage(context.Context, context.Xml, result.ToString());

        _logger.LogDebug("FlushTextMessage: {content}", result.ToString());

        return true;
    }
}
