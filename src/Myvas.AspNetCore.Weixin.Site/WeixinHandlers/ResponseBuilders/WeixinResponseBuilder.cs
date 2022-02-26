using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public static class WeixinResponseBuilder
{
    public static async Task FlushPlainText(HttpContext context, string text)
    {
        context.Response.ContentType = ContentTypeConstants.PlainText;
        await context.Response.WriteAsync(text);
    }

    public static async Task FlushJson(HttpContext context, string json)
    {
        context.Response.ContentType = ContentTypeConstants.Json;
        await context.Response.WriteAsync(json);
    }

    public static async Task FlushHtml(HttpContext context, string html)
    {
        context.Response.ContentType = ContentTypeConstants.Html;
        await context.Response.WriteAsync(html);
    }

    public static async Task FlushStatusCode(HttpContext context, int statusCode = StatusCodes.Status200OK)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = ContentTypeConstants.Html;
        context.Response.Headers.CacheControl = "no-cache,no-store";
        context.Response.Headers.Pragma = "no-cache";
        //await context.Response.CompleteAsync();
        await Task.FromResult(0);
    }

    public static async Task FlushXml(HttpContext context, string xml)
    {
        context.Response.ContentType = ContentTypeConstants.Xml;
        await context.Response.WriteAsync(xml);
    }

    public static async Task FlushTextMessage(HttpContext context, ReceivedXml receivedXml, string text)
    {
        var responseMessage = new WeixinResponseText
        {
            FromUserName = receivedXml.ToUserName,
            ToUserName = receivedXml.FromUserName,
            CreateTime = DateTime.Now
        };
        responseMessage.Content = text;

        var body = WeixinXmlConvert.SerializeObject(responseMessage);
        context.Response.ContentType = ContentTypeConstants.Xml;
        await context.Response.WriteAsync(body);
    }

    public static async Task FlushNewsMessage(HttpContext context, ReceivedXml receivedXml, List<Article> articles)
    {
        var responseMessage = new WeixinResponseNews
        {
            FromUserName = receivedXml.ToUserName,
            ToUserName = receivedXml.FromUserName,
            CreateTime = DateTime.Now
        };
        articles.ForEach(x => responseMessage.Articles.Add(x));

        var body = WeixinXmlConvert.SerializeObject(responseMessage);
        context.Response.ContentType = ContentTypeConstants.Xml;
        await context.Response.WriteAsync(body);
    }
}