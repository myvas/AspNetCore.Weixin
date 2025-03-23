using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Collections.Generic;
using System;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// A <see cref="WeixinEventSink"/> responses a <see cref="ResponseMessageText"/> to the request sender.
/// </summary>
public class WeixinDebugEventSink : WeixinTraceEventSink
{
    /// <inheritdoc/>
    public WeixinDebugEventSink(IOptions<WeixinSiteOptions> optionsAccessor, ILogger<WeixinDebugEventSink> logger) : base(optionsAccessor, logger) { }

    /// <summary>
    /// Flush out a HTTP response with <see cref="WeixinResponseText" />.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="entityInfo"></param>
    /// <returns></returns>
    protected async Task<bool> ResponseWithText(WeixinContext context, ReceivedXml xml, string entityInfo)
    {
        // Get the caller method name from the stack trace, which is also async
        var stackTrace = new StackTrace();
        var frames = new List<StackFrame>();
        for (int i = 1; i < stackTrace.FrameCount; i++)
        {
            frames.Add(stackTrace.GetFrame(i));
        }
        var callerMethodName = "";
        // For async caller, there are many possible methods in frames
        // So we must try to solve it using a loop
        var excludedMethodNames = new HashSet<string>
        {
            nameof(ResponseWithText),
            "MoveNext",
            "Start",
            "SetStateMachine",
            "PerformWaitCallback",
            "ExecuteWithThreadLocal",
            "RunFromThreadPoolDispatchLoop",
            "ExecuteTask",
            "ExecutionContext.Run",
            "Task.ExecuteEntry",
            "Task.RunContinuations",
            "Task.FinishContinuations",
            "Invoke",
            "InvokeMethod",
            "RunInternal",
            "ThreadHelper.ThreadStart",
            "Dispatch"
        };
        foreach (var frame in frames)
        {
            var methodName = frame?.GetMethod()?.Name;
            if (!excludedMethodNames.Contains(methodName))
            {
                callerMethodName = methodName;
                break;
            }
        }
        var entity = new WeixinResponseText($"{callerMethodName}: {entityInfo}")
        {
            ToUserName = xml.FromUserName,
            FromUserName = xml.ToUserName,
            CreateTime = DateTime.Now
        };

        var responseBuilder = new WeixinResponseXmlBuilder(context.Context)
        {
            Content = entity.ToXml()
        };
        await responseBuilder.FlushAsync();
        return true;
    }

    /// <inheritdoc/>
    public override async Task<bool> OnImageMessageReceived(object sender, WeixinEventArgs<ImageMessageReceivedXml> e)
    {
        await base.OnImageMessageReceived(sender, e);
        return await ResponseWithText(e.Context, e.Xml, $"MediaId: {e.Xml.MediaId}, PicUrl: {e.Xml.PicUrl}");
    }

    /// <inheritdoc/>
    public override async Task<bool> OnLinkMessageReceived(object sender, WeixinEventArgs<LinkMessageReceivedXml> e)
    {
        await base.OnLinkMessageReceived(sender, e);
        return await ResponseWithText(e.Context, e.Xml, $"Url: {e.Xml.Url}");
    }

    /// <inheritdoc/>
    public override async Task<bool> OnLocationMessageReceived(object sender, WeixinEventArgs<LocationMessageReceivedXml> e)
    {
        await base.OnLocationMessageReceived(sender, e);
        return await ResponseWithText(e.Context, e.Xml, $"Longitude: {e.Xml.Longitude}, Latitude: {e.Xml.Latitude}, Label: {e.Xml.Label}");
    }

    /// <inheritdoc/>
    public override async Task<bool> OnShortVideoMessageReceived(object sender, WeixinEventArgs<ShortVideoMessageReceivedXml> e)
    {
        await base.OnShortVideoMessageReceived(sender, e);
        return await ResponseWithText(e.Context, e.Xml, $"MediaId: {e.Xml.MediaId}, ThumbMediaId: {e.Xml.ThumbMediaId}");
    }

    /// <inheritdoc/>
    public override async Task<bool> OnTextMessageReceived(object sender, WeixinEventArgs<TextMessageReceivedXml> e)
    {
        await base.OnTextMessageReceived(sender, e);
        return await ResponseWithText(e.Context, e.Xml, $"Content: {e.Xml.Content}");
    }

    /// <inheritdoc/>
    public override async Task<bool> OnVideoMessageReceived(object sender, WeixinEventArgs<VideoMessageReceivedXml> e)
    {
        await base.OnVideoMessageReceived(sender, e);
        return await ResponseWithText(e.Context, e.Xml, $"MediaId: {e.Xml.MediaId}, ThumbMediaId: {e.Xml.ThumbMediaId}");
    }

    /// <inheritdoc/>
    public override async Task<bool> OnVoiceMessageReceived(object sender, WeixinEventArgs<VoiceMessageReceivedXml> e)
    {
        await base.OnVoiceMessageReceived(sender, e);
        return await ResponseWithText(e.Context, e.Xml, $"Format: {e.Xml.Format}, MediaId: {e.Xml.MediaId}, Recognition: {e.Xml.Recognition}");
    }

    /// <inheritdoc/>
    public override async Task<bool> OnClickMenuEventReceived(object sender, WeixinEventArgs<ClickMenuEventReceivedXml> e)
    {
        await base.OnClickMenuEventReceived(sender, e);
        return await ResponseWithText(e.Context, e.Xml, $"EventKey: {e.Xml.EventKey}");
    }

    /// <inheritdoc/>
    public override async Task<bool> OnEnterEventReceived(object sender, WeixinEventArgs<EnterEventReceivedXml> e)
    {
        await base.OnEnterEventReceived(sender, e);
        return await ResponseWithText(e.Context, e.Xml, $"");
    }

    /// <inheritdoc/>
    public override async Task<bool> OnLocationEventReceived(object sender, WeixinEventArgs<LocationEventReceivedXml> e)
    {
        await base.OnLocationEventReceived(sender, e);
        return await ResponseWithText(e.Context, e.Xml, $"Longitude: {e.Xml.Longitude}, Latitude: {e.Xml.Latitude}, Precision: {e.Xml.Precision}");
    }

    /// <inheritdoc/>
    public override async Task<bool> OnQrscanEventReceived(object sender, WeixinEventArgs<QrscanEventReceivedXml> e)
    {
        await base.OnQrscanEventReceived(sender, e);
        return await ResponseWithText(e.Context, e.Xml, $"EventKey: {e.Xml.EventKey}, Ticket: {e.Xml.Ticket}");
    }

    /// <inheritdoc/>
    public override async Task<bool> OnSubscribeEventReceived(object sender, WeixinEventArgs<SubscribeEventReceivedXml> e)
    {
        await base.OnSubscribeEventReceived(sender, e);
        return await ResponseWithText(e.Context, e.Xml, $"EventKey: {e.Xml.EventKey}, Ticket: {e.Xml.Ticket}");
    }

    /// <inheritdoc/>
    public override async Task<bool> OnUnsubscribeEventReceived(object sender, WeixinEventArgs<UnsubscribeEventReceivedXml> e)
    {
        await base.OnUnsubscribeEventReceived(sender, e);
        return await ResponseWithText(e.Context, e.Xml, $"");
    }

    /// <inheritdoc/>
    public override async Task<bool> OnViewMenuEventReceived(object sender, WeixinEventArgs<ViewMenuEventReceivedXml> e)
    {
        await base.OnViewMenuEventReceived(sender, e);
        return await ResponseWithText(e.Context, e.Xml, $"EventKey: {e.Xml.EventKey}");
    }
}
