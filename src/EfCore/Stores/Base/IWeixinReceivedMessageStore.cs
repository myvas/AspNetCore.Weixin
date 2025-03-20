using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Myvas.AspNetCore.Weixin;

public interface IWeixinReceivedMessageStore : IWeixinReceivedMessageStore<WeixinReceivedMessageEntity> { }

/// <summary>
/// Provides an abstraction for storing information of received messages from Weixin subscribers.
/// </summary>
/// <typeparam name="TWeixinReceivedMessageEntity">The type that represents a received message from a Weixin subscriber.</typeparam>
public interface IWeixinReceivedMessageStore<TWeixinReceivedMessageEntity> : IEntityStore<TWeixinReceivedMessageEntity>, IQueryableEntityStore<TWeixinReceivedMessageEntity>
    where TWeixinReceivedMessageEntity : class, IWeixinReceivedMessage, IEntity
{
    Task<IList<TWeixinReceivedMessageEntity>> GetResponseMessagesAsync(string id, int perPage, int pageIndex, CancellationToken cancellationToken = default);

    Task<int> GetCountBySourceOpenIdAsync(string openId, CancellationToken cancellationToken = default);

    Task<IList<TWeixinReceivedMessageEntity>> GetItemsBySourceOpenIdAsync(string openId, int perPage, int pageIndex, CancellationToken cancellationToken = default);
}
