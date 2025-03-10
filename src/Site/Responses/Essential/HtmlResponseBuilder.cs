using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public class HtmlResponseBuilder : ResponseBuilder
{
    public HtmlResponseBuilder(HttpContext context) : base(context)
    {
    }

    public override Task FlushAsync()
    {
        ContentType = ContentTypeConstants.Html;
        return base.FlushAsync();
    }
}
