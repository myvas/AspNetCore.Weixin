using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Myvas.AspNetCore.Weixin;

public abstract class WeixinEventSinkBase : IWeixinEventSink
{
    protected WeixinSiteOptions _options;

    public WeixinEventSinkBase(IOptions<WeixinSiteOptions> optionsAccessor)
    {
        _options = optionsAccessor?.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));
    }

    /// <inheritdoc/>
    public virtual Task<bool> OnImageMessageReceived(object sender, WeixinEventArgs<ImageMessageReceivedXml> e) => _options.Events?.OnImageMessageReceived(sender, e);

    /// <inheritdoc/>
    public virtual Task<bool> OnLinkMessageReceived(object sender, WeixinEventArgs<LinkMessageReceivedXml> e) => _options.Events?.OnLinkMessageReceived(sender, e);

    /// <inheritdoc/>
    public virtual Task<bool> OnLocationMessageReceived(object sender, WeixinEventArgs<LocationMessageReceivedXml> e) => _options.Events?.OnLocationMessageReceived(sender, e);

    /// <inheritdoc/>
    public virtual Task<bool> OnShortVideoMessageReceived(object sender, WeixinEventArgs<ShortVideoMessageReceivedXml> e) => _options.Events?.OnShortVideoMessageReceived(sender, e);

    /// <inheritdoc/>
    public virtual Task<bool> OnTextMessageReceived(object sender, WeixinEventArgs<TextMessageReceivedXml> e) => _options.Events?.OnTextMessageReceived(sender, e);

    /// <inheritdoc/>
    public virtual Task<bool> OnVideoMessageReceived(object sender, WeixinEventArgs<VideoMessageReceivedXml> e) => _options.Events?.OnVideoMessageReceived(sender, e);

    /// <inheritdoc/>
    public virtual Task<bool> OnVoiceMessageReceived(object sender, WeixinEventArgs<VoiceMessageReceivedXml> e) => _options.Events?.OnVoiceMessageReceived(sender, e);

    /// <inheritdoc/>
    public virtual Task<bool> OnClickMenuEventReceived(object sender, WeixinEventArgs<ClickMenuEventReceivedXml> e) => _options.Events?.OnClickMenuEventReceived(sender, e);

    /// <inheritdoc/>
    public virtual Task<bool> OnEnterEventReceived(object sender, WeixinEventArgs<EnterEventReceivedXml> e) => _options.Events?.OnEnterEventReceived(sender, e);

    /// <inheritdoc/>
    public virtual Task<bool> OnLocationEventReceived(object sender, WeixinEventArgs<LocationEventReceivedXml> e) => _options.Events?.OnLocationEventReceived(sender, e);

    /// <inheritdoc/>
    public virtual Task<bool> OnQrscanEventReceived(object sender, WeixinEventArgs<QrscanEventReceivedXml> e) => _options.Events?.OnQrscanEventReceived(sender, e);

    /// <inheritdoc/>
    public virtual Task<bool> OnSubscribeEventReceived(object sender, WeixinEventArgs<SubscribeEventReceivedXml> e) => _options.Events?.OnSubscribeEventReceived(sender, e);

    /// <inheritdoc/>
    public virtual Task<bool> OnUnsubscribeEventReceived(object sender, WeixinEventArgs<UnsubscribeEventReceivedXml> e) => _options.Events?.OnUnsubscribeEventReceived(sender, e);

    /// <inheritdoc/>
    public virtual Task<bool> OnViewMenuEventReceived(object sender, WeixinEventArgs<ViewMenuEventReceivedXml> e) => _options?.Events?.OnViewMenuEventReceived(sender, e);
}
