# Usage: `WeixinEventSink`

## Dependency Injection

```csharp
// <WeixinSiteBuilder>
//.AddWeixinEventSink();
// or customized
.AddWeixinEventSink<YourWeixinEventSink>(o =>
{
	// Setup your options
});
```

## Default `WeixinMessageEvents`

```csharp
OnTextMessageReceived = ctx => weixinEventSink.OnTextMessageReceived(ctx.Sender, ctx.Args),
OnLinkMessageReceived = ctx => weixinEventSink.OnLinkMessageReceived(ctx.Sender, ctx.Args),
OnClickMenuEventReceived = ctx => weixinEventSink.OnClickMenuEventReceived(ctx.Sender, ctx.Args),
OnImageMessageReceived = ctx => weixinEventSink.OnImageMessageReceived(ctx.Sender, ctx.Args),
OnLocationEventReceived = ctx => weixinEventSink.OnLocationEventReceived(ctx.Sender, ctx.Args),
OnLocationMessageReceived = ctx => weixinEventSink.OnLocationMessageReceived(ctx.Sender, ctx.Args),
OnQrscanEventReceived = ctx => weixinEventSink.OnQrscanEventReceived(ctx.Sender, ctx.Args),
OnEnterEventReceived = ctx => weixinEventSink.OnEnterEventReceived(ctx.Sender, ctx.Args),
OnSubscribeEventReceived = ctx => weixinEventSink.OnSubscribeEventReceived(ctx.Sender, ctx.Args),
OnUnsubscribeEventReceived = ctx => weixinEventSink.OnUnsubscribeEventReceived(ctx.Sender, ctx.Args),
OnVideoMessageReceived = ctx => weixinEventSink.OnVideoMessageReceived(ctx.Sender, ctx.Args),
OnShortVideoMessageReceived = ctx => weixinEventSink.OnShortVideoMessageReceived(ctx.Sender, ctx.Args),
OnViewMenuEventReceived = ctx => weixinEventSink.OnViewMenuEventReceived(ctx.Sender, ctx.Args),
OnVoiceMessageReceived = ctx => weixinEventSink.OnVoiceMessageReceived(ctx.Sender, ctx.Args)
```