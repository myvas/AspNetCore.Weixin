using Microsoft.AspNetCore.Http;

namespace Myvas.AspNetCore.Weixin;

public class WeixinRequestContext
{
    public HttpContext RequestContext { get; private set; }

    public string ToUser { get; private set; }
    public string ListenId { get; private set; }
}
