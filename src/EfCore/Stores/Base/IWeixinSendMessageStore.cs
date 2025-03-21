namespace Myvas.AspNetCore.Weixin;

public interface IWeixinSendMessageStore : IWeixinSendMessageStore<WeixinSendMessageEntity> { }

/// <summary>
/// Provides an abstraction for storing information of messages to Weixin subscribers.
/// </summary>
/// <typeparam name="IWeixinSendMessage">The type that represents a message to a Weixin subscriber.</typeparam>
public interface IWeixinSendMessageStore<TWeixinSendMessageEntity> : IEntityStore<TWeixinSendMessageEntity>, IQueryableEntityStore<TWeixinSendMessageEntity>
    where TWeixinSendMessageEntity : class, IWeixinSendMessage, IEntity
{
}
