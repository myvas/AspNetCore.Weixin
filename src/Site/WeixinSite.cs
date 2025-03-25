using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Myvas.AspNetCore.Weixin.Site.Properties;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Myvas.AspNetCore.Weixin;

public class WeixinSite
{
    /// <summary>
    /// The context of the current request and its request body.
    /// </summary>
    protected WeixinContext Context;

    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger _logger;
    private readonly WeixinSiteOptions _options;


    /// <summary>
    /// Construct a new instance of <see cref="WeixinSite"/>.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="text">the text get from the request stream should not be empty</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public WeixinSite(HttpContext context, string text)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        if (string.IsNullOrWhiteSpace(text)) throw new ArgumentNullException(nameof(text));
        Context = new WeixinContext(context, text);

        _serviceProvider = context.RequestServices;
        _logger = _serviceProvider?.GetRequiredService<ILogger<WeixinSite>>() ?? throw new ArgumentNullException(nameof(_logger));
        _options = _serviceProvider?.GetRequiredService<IOptions<WeixinSiteOptions>>()?.Value ?? throw new ArgumentNullException(nameof(_options));
    }

    /// <summary>
    /// Parse the property <see cref="Text"/>, and flush a response.
    /// </summary>
    /// <returns>Always return true.</returns>
    public async Task<bool> HandleAsync()
    {
        var doc = XDocument.Parse(Context.Text);

        RequestMsgType msgType = RequestMsgType.Unknown;
        RequestEventType? eventType = RequestEventType.Unknown;
        try
        {
            string sMsgType = doc.Root.Element("MsgType").Value;
            msgType = (RequestMsgType)Enum.Parse(typeof(RequestMsgType), sMsgType, true);
        }
        catch { msgType = RequestMsgType.Unknown; }
        if (msgType == RequestMsgType.@event)
        {
            var sEventType = doc.Root.Element("Event").Value;
            try
            {
                eventType = (RequestEventType)Enum.Parse(typeof(RequestEventType), sEventType, true);
                msgType = RequestMsgType.@event;
            }
            catch { eventType = RequestEventType.Unknown; }
        }

        try
        {
            switch (msgType)
            {
                case RequestMsgType.text:
                    return await FireEventAsync<TextMessageReceivedXml>(nameof(IWeixinEventSink.OnTextMessageReceived));
                case RequestMsgType.image:
                    return await FireEventAsync<ImageMessageReceivedXml>(nameof(IWeixinEventSink.OnImageMessageReceived));
                case RequestMsgType.voice:
                    return await FireEventAsync<VoiceMessageReceivedXml>(nameof(IWeixinEventSink.OnVoiceMessageReceived));
                case RequestMsgType.video:
                    return await FireEventAsync<VideoMessageReceivedXml>(nameof(IWeixinEventSink.OnVideoMessageReceived));
                case RequestMsgType.shortvideo:
                    return await FireEventAsync<ShortVideoMessageReceivedXml>(nameof(IWeixinEventSink.OnShortVideoMessageReceived));
                case RequestMsgType.location:
                    return await FireEventAsync<LocationMessageReceivedXml>(nameof(IWeixinEventSink.OnLocationMessageReceived));
                case RequestMsgType.link:
                    return await FireEventAsync<LinkMessageReceivedXml>(nameof(IWeixinEventSink.OnLinkMessageReceived));
                case RequestMsgType.@event:
                    switch (eventType)
                    {
                        case RequestEventType.subscribe:
                            return await FireEventAsync<SubscribeEventReceivedXml>(nameof(IWeixinEventSink.OnSubscribeEventReceived));
                        case RequestEventType.LOCATION:
                            return await FireEventAsync<LocationEventReceivedXml>(nameof(IWeixinEventSink.OnLocationEventReceived));
                        case RequestEventType.CLICK:
                            return await FireEventAsync<ClickMenuEventReceivedXml>(nameof(IWeixinEventSink.OnClickMenuEventReceived));
                        case RequestEventType.VIEW:
                            return await FireEventAsync<ViewMenuEventReceivedXml>(nameof(IWeixinEventSink.OnViewMenuEventReceived));
                        case RequestEventType.SCAN:
                            return await FireEventAsync<QrscanEventReceivedXml>(nameof(IWeixinEventSink.OnQrscanEventReceived));
                        case RequestEventType.unsubscribe:
                            return await FireEventAsync<UnsubscribeEventReceivedXml>(nameof(IWeixinEventSink.OnUnsubscribeEventReceived));
                        default:
                            // 系统无法识别处理此事件
                            throw new NotSupportedException($"{Resources.UnknownRequestMessageEvent}");
                    }
                default:
                    // 系统无法识别处理此消息
                    throw new NotSupportedException($"{Resources.UnknownRequstMessage}");
            }
        }
        catch (Exception ex)
        {
            // 系统在解析处理微信消息时发生异常
            throw new NotSupportedException($"{Resources.ExceptionOnHandler}", ex);
        }
    }

    private async Task<bool> FireEventAsync<TReceivedXml>(string methodName)
        where TReceivedXml : ReceivedXml
    {
        var msg = "";
        var receivedXml = WeixinXmlConvert.DeserializeObject<TReceivedXml>(Context.Text);
        var ctx = new WeixinEventArgs<TReceivedXml>(Context, receivedXml);
        var handled = false;
        using (var scope = _serviceProvider.CreateScope())
        {
            var handler = scope.ServiceProvider.GetRequiredService<IWeixinEventSink>();
            if (handler != null)
            {
                // If response is already sent, handled is true.
                handled = await CallHandlerMethodAsync(handler, methodName, ctx);
                if (handled) return true;
            }
            else
            {
                msg = $"No handler found in the service provider.";
                Trace.WriteLine(msg);
                _logger.LogError(msg);
                return await Response501NotImplementedAsync();
            }
        }
        if (!handled)
        {
            msg = $"Probably the handler does not correctly process the received xml: {receivedXml}";
            Trace.WriteLine(msg);
            _logger.LogError(msg);
            return await Response501NotImplementedAsync();
        }
        return false;
    }

    private async Task<bool> CallHandlerMethodAsync<TReceivedXml>(IWeixinEventSink handler, string methodName, WeixinEventArgs<TReceivedXml> ctx)
        where TReceivedXml : ReceivedXml
    {
        var msg = "";
        try
        {
            var type = handler.GetType();
            var method = type.GetMethod(methodName);
            if (method == null)
            {
                msg = $"Method '{methodName}' not found in handler.";
                Debug.WriteLine(msg);
                _logger.LogTrace(msg);
                return false;
            }
            if (method.ReturnType != typeof(Task<bool>))
            {
                msg = $"Method '{methodName}' does not return Task<bool>.";
                Trace.WriteLine(msg);
                _logger.LogError(msg);
                return false;
            }
            var task = (Task<bool>)method.Invoke(handler, [this, ctx]);
            return await task;
        }
        catch (TargetInvocationException tie)
        {
            msg = $"Method '{methodName}' threw an exception: {tie.InnerException?.Message}";
            Trace.WriteLine(msg);
            _logger.LogError(msg);
            return false;
        }
        catch (Exception ex)
        {
            msg = $"Unexpected error calling '{methodName}': {ex.Message}";
            Trace.WriteLine(msg);
            _logger.LogError(msg);
            return false;
        }
    }

    /// <summary>
    /// Flush a response with status code <see cref="StatusCodes.Status200OK"/>.
    /// </summary>
    /// <returns></returns>
    protected virtual async Task<bool> DefaultResponseAsync()
    {
        var responseBuilder = new WeixinResponsePlainTextBuilder(Context.Context);
        responseBuilder.Content = Resources.ErrorOnCallingHandler;
        await responseBuilder.FlushAsync();
        return true;
    }

    protected virtual async Task<bool> Response500InternalServerErrorAsync(string content = null)
    {
        var responseBuilder = new WeixinResponsePlainTextBuilder(Context.Context);
        responseBuilder.StatusCode = StatusCodes.Status500InternalServerError;
        responseBuilder.Content = content ?? Resources.Response500InternalServerError;
        await responseBuilder.FlushAsync();
        return true;
    }

    protected virtual async Task<bool> Response501NotImplementedAsync(string content = null)
    {
        var responseBuilder = new WeixinResponsePlainTextBuilder(Context.Context);
        responseBuilder.StatusCode = StatusCodes.Status501NotImplemented;
        responseBuilder.Content = content ?? Resources.Response501NotImplemented;
        await responseBuilder.FlushAsync();
        return true;
    }
}
