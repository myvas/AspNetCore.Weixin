using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// Create a 
/// </summary>
/// <typeparam name="TWeixinResponseMessage"></typeparam>
public class WeixinResponseMessageBuilder<TWeixinResponseMessage> : WeixinResponseXmlBuilder
    where TWeixinResponseMessage : IWeixinResponseMessage, new()
{
    public WeixinResponseMessageBuilder(HttpContext context, ReceivedXml receivedXml, long? createTime = null) : base(context)
    {
        ReceivedXml = receivedXml;
        ResponseMessage = new TWeixinResponseMessage();
        ResponseMessage.FromUserName = receivedXml.ToUserName;
        ResponseMessage.ToUserName = receivedXml.FromUserName;
        ResponseMessage.CreateTime = new UnixDateTime(createTime ?? DateTime.Now.ToUnixTime());
    }

    public ReceivedXml ReceivedXml { get; private set; }
    public TWeixinResponseMessage ResponseMessage { get; private set; }

    public override Task FlushAsync()
    {
        Content = ResponseMessage.ToXml();
        return base.FlushAsync();
    }
}
