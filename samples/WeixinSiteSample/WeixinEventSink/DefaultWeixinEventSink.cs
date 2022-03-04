using Microsoft.Extensions.Logging;
using Myvas.AspNetCore.Weixin;
using System.Text;
using System.Threading.Tasks;

namespace WeixinSiteSample;

public class DefaultWeixinEventSink : WeixinEventSink
{
    public DefaultWeixinEventSink(IWeixinResponseBuilder responseBuilder, ILogger<IWeixinEventSink> logger) : base(responseBuilder, logger)
    {
    }

    public override async Task<bool> OnTextMessageReceived(WeixinEventContext<TextMessageReceivedXml> context)
    {
        _logger.LogTrace("OnTextMessageReceived: {content}", context.Xml.Content);

        var result = new StringBuilder();
        result.AppendFormat("收到一条微信文本消息：{0}", context.Xml.Content);

        await WeixinResponseBuilder.FlushTextMessage(context.Context, context.Xml, result.ToString());

        _logger.LogDebug("FlushTextMessage: {content}", result.ToString());

        return true;
    }

    public override async Task<bool> OnImageMessageReceived(WeixinEventContext<ImageMessageReceivedXml> context)
    {
        _logger.LogTrace("OnImageMessageReceived: {mediaId}, {picUrl}", context.Xml.MediaId, context.Xml.PicUrl);

        var result = new StringBuilder();
        result.AppendFormat("收到一条微信图片消息：{0}, {1}", context.Xml.MediaId, context.Xml.PicUrl);

        await WeixinResponseBuilder.FlushTextMessage(context.Context, context.Xml, result.ToString());

        _logger.LogDebug("FlushTextMessage: {content}", result.ToString());

        return true;
    }
}
