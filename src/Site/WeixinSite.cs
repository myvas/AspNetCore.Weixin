using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Myvas.AspNetCore.Weixin;

public class WeixinSite : IWeixinSite
{
    private readonly ILogger _logger;
    private readonly WeixinSiteOptions _options;

    public WeixinContext Context { get; set; }

    private readonly IServiceProvider _serviceProvider;

    private readonly IEnumerable<IWeixinEventSink> _handlers;

    public WeixinSite(ILoggerFactory logger,
        IOptions<WeixinSiteOptions> optionsAccessor,
        IServiceProvider serviceProvider)
    {
        _logger = logger?.CreateLogger<WeixinSite>() ?? throw new ArgumentNullException(nameof(logger));
        _options = optionsAccessor?.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _handlers = _serviceProvider.GetServices<IWeixinEventSink>();
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
        var receivedXml = WeixinXmlConvert.DeserializeObject<TReceivedXml>(Context.Text);
        var ctx = new WeixinEventArgs<TReceivedXml>(Context, receivedXml);
        var handled = false;
        foreach (var handler in _handlers)
        {
            handled = await CallHandlerMethodAsync(handler, methodName, ctx);
            if (handled) return true;
        }
        if (!handled)
        {
            _logger.LogWarning($"Probably missing a handler when processing the received xml: {receivedXml}");
            return await DefaultResponseAsync();
        }
        return false;
    }

    private async Task<bool> CallHandlerMethodAsync<TReceivedXml>(IWeixinEventSink handler, string methodName, WeixinEventArgs<TReceivedXml> ctx)
        where TReceivedXml : ReceivedXml
    {
        try
        {
            var type = handler.GetType();
            var method = type.GetMethod(methodName);
            if (method == null)
            {
                _logger.LogError($"An error occurred on the server when calling a handler without a specific method [{methodName}]");
            }
            var task = (Task<bool>)method.Invoke(handler, [this, ctx]);
            return await task;
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"An error occurred on the server when the handler [{handler.GetType().Name}] try to process the received xml: {ctx.Xml}.");
            _logger.LogError(ex.Message);
            _logger.LogTrace(ex.StackTrace);
        }
        return false;
    }

    /// <summary>
    /// Flush a response with status code <see cref="StatusCodes.Status200OK"/>.
    /// </summary>
    /// <returns></returns>
    protected virtual async Task<bool> DefaultResponseAsync()
    {
        var responseBuilder = new PlainTextResponseBuilder(Context.Context);
        responseBuilder.Content = Resources.ErrorOnCallingHandler;
        await responseBuilder.FlushAsync();
        return true;
    }
}
