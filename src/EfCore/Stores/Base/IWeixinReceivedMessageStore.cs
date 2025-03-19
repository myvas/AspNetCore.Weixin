using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Myvas.AspNetCore.Weixin;

public interface IWeixinReceivedMessageStore : IWeixinReceivedMessageStore<WeixinReceivedMessage> { }

/// <summary>
/// Provides an abstraction for storing information of received messages from Weixin subscribers.
/// </summary>
/// <typeparam name="TWeixinReceivedMessage">The type that represents a received message from a Weixin subscriber.</typeparam>
public interface IWeixinReceivedMessageStore<TWeixinReceivedMessage> : IEntityStore<TWeixinReceivedMessage>, IQueryableEntityStore<TWeixinReceivedMessage>
    where TWeixinReceivedMessage : class, IEntity
{
    Task<IList<TWeixinReceivedMessage>> GetResponseMessagesAsync(string id, int perPage, int pageIndex, CancellationToken cancellationToken = default);

    Task<int> GetCountBySourceOpenIdAsync(string openId, CancellationToken cancellationToken = default);

    Task<IList<TWeixinReceivedMessage>> GetItemsBySourceOpenIdAsync(string openId, int perPage, int pageIndex, CancellationToken cancellationToken = default);
}
