using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public static class HttpResponseWriteWithHeadersExtensions
{
    public static async Task FlushPlainText(this HttpResponse Response, string text)
    {
        Response.ContentType = ContentTypeConstants.PlainText;

        await Response.WriteAsync(text);
    }

    public static async Task FlushJson(this HttpResponse Response, string json)
    {
        Response.ContentType = ContentTypeConstants.Json;

        await Response.WriteAsync(json);
    }

    public static async Task FlushHtml(this HttpResponse Response, string html)
    {
        Response.ContentType = ContentTypeConstants.Html;

        await Response.WriteAsync(html);
    }

    public static async Task FlushStatusCode(this HttpResponse Response, int statusCode = StatusCodes.Status204NoContent, string html = "", bool complete = true)
    {
        Response.ContentType = ContentTypeConstants.Html;

        Response.StatusCode = statusCode;
        Response.Headers.SetCacheControl("no-cache,no-store");
        Response.Headers.SetPragma("no-cache");

        if (!string.IsNullOrEmpty(html))
        {
            await Response.WriteAsync(html);
        }
        if (complete)
        {
            await Response.Body.FlushAsync();
        }

        await Task.FromResult(0);
    }

    public static async Task FlushXml(this HttpResponse Response, string xml)
    {
        Response.ContentType = ContentTypeConstants.Xml;

        await Response.WriteAsync(xml);
    }

    public static async Task FlushTextMessage(this HttpResponse Response, ReceivedXml receivedXml, string text)
    {
        var responseMessage = new WeixinResponseText
        {
            FromUserName = receivedXml.ToUserName,
            ToUserName = receivedXml.FromUserName,
            CreateTime = DateTime.Now
        };
        responseMessage.Content = text;

        var body = WeixinXmlConvert.SerializeObject(responseMessage);
        Response.ContentType = ContentTypeConstants.Xml;

        await Response.WriteAsync(body);
    }

    public static async Task FlushNewsMessage(this HttpResponse Response, ReceivedXml receivedXml, List<WeixinResponseNewsArticle> articles)
    {
        var responseMessage = new WeixinResponseNews
        {
            FromUserName = receivedXml.ToUserName,
            ToUserName = receivedXml.FromUserName,
            CreateTime = DateTime.Now
        };
        articles.ForEach(x => responseMessage.Articles.Add(x));

        var body = WeixinXmlConvert.SerializeObject(responseMessage);
        Response.ContentType = ContentTypeConstants.Xml;

        await Response.WriteAsync(body);
    }
}