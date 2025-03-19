# Usage: `WeixinEventSink`

## Default `WeixinDebugEventSink`

```csharp
.AddWeixinSite(...);

```

## EntityFrameworkCore `WeixinEfCoreEventSink`
```csharp
.AddEfCore<WeixinDbContext>();
```

## Customize and Dependency Injection

```csharp
public class YourWeixinEfCoreEventSink<TWeixinSubscriber, TKey> : WeixinDebugEventSink // or IWeixinEventSink
    where TWeixinSubscriber : WeixinSubscriber<TKey>, new()
    where TKey : IEquatable<TKey>
{

    protected readonly IWeixinReceivedEventStore<WeixinReceivedEvent> _eventStore;
    protected readonly IWeixinReceivedMessageStore<WeixinReceivedMessage> _messageStore;
    protected readonly IWeixinSubscriberStore<TWeixinSubscriber, TKey> _subscriberStore;

    public YourWeixinEfCoreEventSink(IOptions<WeixinSiteOptions> optionsAccessor,
		ILogger<YourWeixinEfCoreEventSink<TWeixinSubscriber, TKey>> logger,
        IWeixinReceivedMessageStore<WeixinReceivedMessage> messageStore,
        IWeixinReceivedEventStore<WeixinReceivedEvent> eventStore,
        IWeixinSubscriberStore<TWeixinSubscriber, TKey> subscriberStore)
        : base(optionsAccessor, logger)
    {
        _messageStore = messageStore ?? throw new ArgumentNullException(nameof(messageStore));
        _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
        _subscriberStore = subscriberStore ?? throw new ArgumentNullException(nameof(subscriberStore));
    }

	/// <inheritdoc/>
    public override async Task<bool> OnSubscribeEventReceived(object sender, WeixinEventArgs<SubscribeEventReceivedXml> e)
    {
        // Store
        try
        {
            var createResult = await _eventStore.CreateAsync(e.Xml.ToEvent());
            _logger.LogTrace("已将微信订阅事件存入数据库。{subscriber}, {eventKey}", e.Xml.FromUserName, e.Xml.EventKey);

            var entity = await _subscriberStore.FindByOpenIdAsync(e.Xml.FromUserName);
            if (entity == null)
            {
                entity = new TWeixinSubscriber()
                {
                    OpenId = e.Xml.FromUserName,
                    SubscribedTime = DateTimeOffset.Now,
                    Unsubscribed = false
                };
                var subscribeResult = await _subscriberStore.CreateAsync(entity);
                _logger.LogTrace("已将新的微信订阅者存入数据库。{subscriber}, {eventKey}", e.Xml.FromUserName, e.Xml.EventKey);
            }
            else
            {
                entity.SubscribedTime = DateTimeOffset.Now;
                entity.Unsubscribed = false;
                var resubscribeResult = await _subscriberStore.UpdateAsync(entity);
                _logger.LogTrace("已将现有微信订阅者更新订阅标记。{subscriber}, {eventKey}", e.Xml.FromUserName, e.Xml.EventKey);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning("将微信订阅事件存入数据库时发生异常。");
            _logger.LogDebug(ex, ex.Message);
            _logger.LogTrace(ex.InnerException ?? ex, ex.InnerException?.Message ?? ex.Message);
        }
        return await base.OnSubscribeEventReceived(sender, e);
    }
	
	// ...
}

// <WeixinSiteBuilder>
.AddWeixinEventSink<YourWeixinEventSink>(o =>
{
	// Setup your options
});
```