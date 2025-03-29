using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public abstract class WeixinResponseBuilder : IWeixinResponseBuilder
{
    public HttpContext Context { get; }
    public string ContentType { get; set; }
    public string Content { get; set; }
    public int StatusCode { get; set; } = StatusCodes.Status200OK;

    public WeixinResponseBuilder(HttpContext context)
    {
        Context = context;
    }

    public virtual async Task FlushAsync()
    {
        Context.Response.StatusCode = StatusCode;
        Context.Response.ContentType = ContentType;
        await Context.Response.WriteAsync(Content);
        Debug.WriteLine($"Response via {this.GetType().Name}:");
        Debug.WriteLine(Content);
    }
}
