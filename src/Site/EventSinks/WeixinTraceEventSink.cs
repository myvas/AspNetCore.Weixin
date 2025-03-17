using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;
/// <summary>
/// A <see cref="WeixinEventSink"/> logging the trace of current method name and event data.
/// </summary>
public class WeixinTraceEventSink : WeixinEventSinkBase
{
    protected readonly ILogger _logger;

    /// <inheritdoc/>
    public WeixinTraceEventSink(IOptions<WeixinSiteOptions> optionsAccessor, ILogger<WeixinTraceEventSink> logger)
        : base(optionsAccessor)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Log trace of current method name, the sender, and the event data.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    protected Task<bool> PrintTrace(object sender, object obj)
    {
        // code info
        var stackTrace = new StackTrace();
        var callerFrame = stackTrace.GetFrame(1);
        var callerMethodName = callerFrame.GetMethod().Name;
        _logger.LogTrace(callerMethodName);

        // sender info
        _logger.LogTrace($"sender: {sender?.ToString()}[type: {sender?.GetType().Name}]");

        // event info
        _logger.LogTrace(WeixinXmlConvert.SerializeObject(obj));

        return Task.FromResult(false);
    }

    /// <inheritdoc/>
    public override async Task<bool> OnImageMessageReceived(object sender, WeixinEventArgs<ImageMessageReceivedXml> e) => (await base.OnImageMessageReceived(sender, e)) && (await PrintTrace(sender, e.Xml));

    /// <inheritdoc/>
    public override async Task<bool> OnLinkMessageReceived(object sender, WeixinEventArgs<LinkMessageReceivedXml> e) => (await base.OnLinkMessageReceived(sender, e)) && (await PrintTrace(sender, e.Xml));

    /// <inheritdoc/>
    public override async Task<bool> OnLocationMessageReceived(object sender, WeixinEventArgs<LocationMessageReceivedXml> e) => (await base.OnLocationMessageReceived(sender, e)) && (await PrintTrace(sender, e.Xml));

    /// <inheritdoc/>
    public override async Task<bool> OnShortVideoMessageReceived(object sender, WeixinEventArgs<ShortVideoMessageReceivedXml> e) => (await base.OnShortVideoMessageReceived(sender, e)) && (await PrintTrace(sender, e.Xml));

    /// <inheritdoc/>
    public override async Task<bool> OnTextMessageReceived(object sender, WeixinEventArgs<TextMessageReceivedXml> e) => (await base.OnTextMessageReceived(sender, e)) && (await PrintTrace(sender, e.Xml));

    /// <inheritdoc/>
    public override async Task<bool> OnVideoMessageReceived(object sender, WeixinEventArgs<VideoMessageReceivedXml> e) => (await base.OnVideoMessageReceived(sender, e)) && (await PrintTrace(sender, e.Xml));

    /// <inheritdoc/>
    public override async Task<bool> OnVoiceMessageReceived(object sender, WeixinEventArgs<VoiceMessageReceivedXml> e) => (await base.OnVoiceMessageReceived(sender, e)) && (await PrintTrace(sender, e.Xml));

    /// <inheritdoc/>
    public override async Task<bool> OnClickMenuEventReceived(object sender, WeixinEventArgs<ClickMenuEventReceivedXml> e) => (await base.OnClickMenuEventReceived(sender, e)) && (await PrintTrace(sender, e.Xml));

    /// <inheritdoc/>
    public override async Task<bool> OnEnterEventReceived(object sender, WeixinEventArgs<EnterEventReceivedXml> e) => (await base.OnEnterEventReceived(sender, e)) && (await PrintTrace(sender, e.Xml));

    /// <inheritdoc/>
    public override async Task<bool> OnLocationEventReceived(object sender, WeixinEventArgs<LocationEventReceivedXml> e) => (await base.OnLocationEventReceived(sender, e)) && (await PrintTrace(sender, e.Xml));

    /// <inheritdoc/>
    public override async Task<bool> OnQrscanEventReceived(object sender, WeixinEventArgs<QrscanEventReceivedXml> e) => (await base.OnQrscanEventReceived(sender, e)) && (await PrintTrace(sender, e.Xml));

    /// <inheritdoc/>
    public override async Task<bool> OnSubscribeEventReceived(object sender, WeixinEventArgs<SubscribeEventReceivedXml> e) => (await base.OnSubscribeEventReceived(sender, e)) && (await PrintTrace(sender, e.Xml));

    /// <inheritdoc/>
    public override async Task<bool> OnUnsubscribeEventReceived(object sender, WeixinEventArgs<UnsubscribeEventReceivedXml> e) => (await base.OnUnsubscribeEventReceived(sender, e)) && (await PrintTrace(sender, e.Xml));

    /// <inheritdoc/>
    public override async Task<bool> OnViewMenuEventReceived(object sender, WeixinEventArgs<ViewMenuEventReceivedXml> e) => (await base.OnViewMenuEventReceived(sender, e)) && (await PrintTrace(sender, e.Xml));
}
