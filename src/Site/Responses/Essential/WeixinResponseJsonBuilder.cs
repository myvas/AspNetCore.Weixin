using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public class WeixinResponseJsonBuilder : WeixinResponseBuilder
{
    public WeixinResponseJsonBuilder(HttpContext context) : base(context)
    {
    }

    public override Task FlushAsync()
    {
        ContentType = ContentTypeConstants.Json;
        return base.FlushAsync();
    }
}
