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
    protected void PrintTrace(object sender, object obj)
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
    }

    #region Uplink/Messages

    /// <inheritdoc/>
    public override async Task<bool> OnTextMessageReceived(object sender, WeixinEventArgs<TextMessageReceivedXml> e)
    {
        PrintTrace(sender, e.Xml);
        await base.OnTextMessageReceived(sender, e);
        return false;
    }

    /// <inheritdoc/>
    public override async Task<bool> OnLinkMessageReceived(object sender, WeixinEventArgs<LinkMessageReceivedXml> e)
    {
        PrintTrace(sender, e.Xml);
        await base.OnLinkMessageReceived(sender, e);
        return false;
    }

    /// <inheritdoc/>
    public override async Task<bool> OnVideoMessageReceived(object sender, WeixinEventArgs<VideoMessageReceivedXml> e)
    {
        PrintTrace(sender, e.Xml);
        await base.OnVideoMessageReceived(sender, e);
        return false;
    }

    /// <inheritdoc/>
    public override async Task<bool> OnShortVideoMessageReceived(object sender, WeixinEventArgs<ShortVideoMessageReceivedXml> e)
    {
        PrintTrace(sender, e.Xml);
        await base.OnShortVideoMessageReceived(sender, e);
        return false;
    }

    /// <inheritdoc/>
    public override async Task<bool> OnVoiceMessageReceived(object sender, WeixinEventArgs<VoiceMessageReceivedXml> e)
    {
        PrintTrace(sender, e.Xml);
        await base.OnVoiceMessageReceived(sender, e);
        return false;
    }

    /// <inheritdoc/>
    public override async Task<bool> OnImageMessageReceived(object sender, WeixinEventArgs<ImageMessageReceivedXml> e)
    {
        PrintTrace(sender, e.Xml);
        await base.OnImageMessageReceived(sender, e);
        return false;
    }

    /// <inheritdoc/>
    public override async Task<bool> OnLocationMessageReceived(object sender, WeixinEventArgs<LocationMessageReceivedXml> e)
    {
        PrintTrace(sender, e.Xml);
        await base.OnLocationMessageReceived(sender, e);
        return false;
    }

    #endregion
    #region Uplink/Events

    /// <inheritdoc/>
    public override async Task<bool> OnLocationEventReceived(object sender, WeixinEventArgs<LocationEventReceivedXml> e)
    {
        PrintTrace(sender, e.Xml);
        await base.OnLocationEventReceived(sender, e);
        return false;
    }

    /// <inheritdoc/>
    public override async Task<bool> OnClickMenuEventReceived(object sender, WeixinEventArgs<ClickMenuEventReceivedXml> e)
    {
        PrintTrace(sender, e.Xml);
        await base.OnClickMenuEventReceived(sender, e);
        return false;
    }

    /// <inheritdoc/>
    public override async Task<bool> OnViewMenuEventReceived(object sender, WeixinEventArgs<ViewMenuEventReceivedXml> e)
    {
        PrintTrace(sender, e.Xml);
        await base.OnViewMenuEventReceived(sender, e);
        return false;
    }

    /// <inheritdoc/>
    public override async Task<bool> OnUnsubscribeEventReceived(object sender, WeixinEventArgs<UnsubscribeEventReceivedXml> e)
    {
        PrintTrace(sender, e.Xml);
        await base.OnUnsubscribeEventReceived(sender, e);
        return false;
    }

    /// <inheritdoc/>
    public override async Task<bool> OnQrscanEventReceived(object sender, WeixinEventArgs<QrscanEventReceivedXml> e)
    {
        PrintTrace(sender, e.Xml);
        await base.OnQrscanEventReceived(sender, e);
        return false;
    }

    /// <inheritdoc/>
    public override async Task<bool> OnSubscribeEventReceived(object sender, WeixinEventArgs<SubscribeEventReceivedXml> e)
    {
        PrintTrace(sender, e.Xml);
        await base.OnSubscribeEventReceived(sender, e);
        return false;
    }

    /// <inheritdoc/>
    public override async Task<bool> OnEnterEventReceived(object sender, WeixinEventArgs<EnterEventReceivedXml> e)
    {
        PrintTrace(sender, e.Xml);
        await base.OnEnterEventReceived(sender, e);
        return false;
    }

    #endregion
}
