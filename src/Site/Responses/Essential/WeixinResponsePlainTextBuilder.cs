using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public class WeixinResponsePlainTextBuilder : WeixinResponseBuilder
{
    public WeixinResponsePlainTextBuilder(HttpContext context) : base(context)
    {
    }

    public override Task FlushAsync()
    {
        ContentType = ContentTypeConstants.PlainText;
        return base.FlushAsync();
    }
}
