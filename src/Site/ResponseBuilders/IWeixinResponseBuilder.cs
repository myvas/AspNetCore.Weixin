using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public interface IWeixinResponseBuilder
{
    Task FlushHtml(HttpContext context, string html);
    Task FlushJson(HttpContext context, string json);
    Task FlushNewsMessage(HttpContext context, ReceivedXml receivedXml, List<Article> articles);
    Task FlushPlainText(HttpContext context, string text);
    Task FlushStatusCode(HttpContext context, int statusCode = StatusCodes.Status200OK);
    Task FlushTextMessage(HttpContext context, ReceivedXml receivedXml, string text);
    Task FlushXml(HttpContext context, string xml);
}