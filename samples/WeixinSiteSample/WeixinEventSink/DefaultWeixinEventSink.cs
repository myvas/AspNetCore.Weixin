using Myvas.AspNetCore.Weixin;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore;
using Myvas.AspNetCore.Weixin.Models;
using System.Text;

namespace WeixinSiteSample;

public class DefaultWeixinEventSink : WeixinEventSink
{
    public DefaultWeixinEventSink(ILogger<IWeixinEventSink> logger, IWeixinResponseBuilder responseBuilder, IReceivedEntryStore<EventReceivedEntry> eventStore, IReceivedEntryStore<MessageReceivedEntry> messageStore) : base(logger, responseBuilder, eventStore, messageStore)
    {
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
