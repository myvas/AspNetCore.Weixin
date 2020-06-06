using Microsoft.AspNetCore.Http;
using System;

namespace Myvas.AspNetCore.Weixin
{
    public class WeixinResponseBuilder<TWeixinResponse> : XmlResponseBuilder
        where TWeixinResponse : IWeixinResponse, new()
    {
        public WeixinResponseBuilder(HttpContext context) : base(context)
        {
        }

        public TWeixinResponse Response { get; private set; }
        public ReceivedXml ReceivedXml { get; private set; }

        public virtual void Init(ReceivedXml receivedXml)
        {
            ReceivedXml = receivedXml;
            Response = new TWeixinResponse();
            Response.FromUserName = receivedXml.ToUserName;
            Response.ToUserName = receivedXml.FromUserName;
            Response.CreateTime = DateTime.Now;
        }
    }

}
