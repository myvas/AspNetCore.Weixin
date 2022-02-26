using Microsoft.Extensions.Logging;
using Myvas.AspNetCore.Weixin;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin
{
    public class DefaultWeixinEventSink : WeixinEventSinkBase
    {
        public DefaultWeixinEventSink(IWeixinResponseBuilder responseBuilder, ILogger<WeixinEventSinkBase> logger) : base(responseBuilder, logger)
        {
        }
    }
}
