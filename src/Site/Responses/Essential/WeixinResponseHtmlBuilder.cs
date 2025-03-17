using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public class WeixinResponseHtmlBuilder : WeixinResponseBuilder
{
    public WeixinResponseHtmlBuilder(HttpContext context) : base(context)
    {
    }

    public override Task FlushAsync()
    {
        ContentType = ContentTypeConstants.Html;
        return base.FlushAsync();
    }
}
