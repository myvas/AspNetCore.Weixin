using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public class WeixinResponseXmlBuilder : WeixinResponseBuilder
{
    public WeixinResponseXmlBuilder(HttpContext context) : base(context)
    {
    }

    public override Task FlushAsync()
    {
        ContentType = ContentTypeConstants.Xml;
        return base.FlushAsync();
    }
}
