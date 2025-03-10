using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public class WeixinResponseBuilder<TWeixinResponse> : XmlResponseBuilder
    where TWeixinResponse : IWeixinResponse, new()
{
    public WeixinResponseBuilder(HttpContext context, ReceivedXml receivedXml) : base(context)
    {
        ReceivedXml = receivedXml;
        ResponseEntity = new TWeixinResponse();
        ResponseEntity.FromUserName = receivedXml.ToUserName;
        ResponseEntity.ToUserName = receivedXml.FromUserName;
        ResponseEntity.CreateTime = DateTime.Now;
    }

    public ReceivedXml ReceivedXml { get; private set; }
    public TWeixinResponse ResponseEntity { get; private set; }

    public override Task FlushAsync()
    {
        Content = ResponseEntity.ToXml();
        return base.FlushAsync();
    }
}
