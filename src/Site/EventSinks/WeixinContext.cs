using Microsoft.AspNetCore.Http;

namespace Myvas.AspNetCore.Weixin;

public class WeixinContext
{
    /// <summary>
    /// The <see cref="HttpContext"/> from request.
    /// </summary>
    public HttpContext Context { get; set; }

    /// <summary>
    /// The string parsed from the request body of <see cref="Context"/>.
    /// </summary>
    public string Text { get; set; }

    public WeixinContext()
    {
    }
    
    public WeixinContext(HttpContext context)
    {
        Context = context;
    }

    public WeixinContext(HttpContext context, string text)
    {
        (Context, Text) = (context, text);
    }
}
