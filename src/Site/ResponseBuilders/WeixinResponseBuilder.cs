using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Myvas.AspNetCore.Weixin.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public class WeixinResponseBuilder : IWeixinResponseBuilder
{
    private readonly ILogger _logger;

    public WeixinResponseBuilder(ILogger<WeixinResponseBuilder> logger)
    {
        _logger = logger;
    }

    public async Task FlushPlainText(HttpContext context, string text)
    {
        context.Response.ContentType = ContentTypeConstants.PlainText;

        await WriteAsync(context, text);
    }

    public async Task FlushJson(HttpContext context, string json)
    {
        context.Response.ContentType = ContentTypeConstants.Json;

        await WriteAsync(context, json);
    }

    public async Task FlushHtml(HttpContext context, string html)
    {
        context.Response.ContentType = ContentTypeConstants.Html;

        await WriteAsync(context, html);
    }

    public async Task FlushStatusCode(HttpContext context, int statusCode = StatusCodes.Status200OK)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = ContentTypeConstants.Html;
        context.Response.Headers.SetCacheControl("no-cache,no-store");
        context.Response.Headers.SetPragma("no-cache");
        //await context.Response.CompleteAsync();

        _logger.LogTrace("Response StatusCode: {statusCode}", statusCode);
        await Task.FromResult(0);
    }

    public async Task FlushXml(HttpContext context, string xml)
    {
        context.Response.ContentType = ContentTypeConstants.Xml;

        await WriteAsync(context, xml);
    }

    public async Task FlushTextMessage(HttpContext context, ReceivedXml receivedXml, string text)
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

        await WriteAsync(context, body);
    }

    public async Task FlushNewsMessage(HttpContext context, ReceivedXml receivedXml, List<Article> articles)
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

        await WriteAsync(context, body);
    }

    private async Task WriteAsync(HttpContext context, string text)
    {
        if (!string.IsNullOrWhiteSpace(text))
        {
            await context.Response.WriteAsync(text);
            _logger.LogTrace("Response Body({length}): {text}", text?.Length, text);
        }
    }
}