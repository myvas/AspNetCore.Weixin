using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Myvas.AspNetCore.Weixin;

public interface IWeixinReceivedEventStore : IWeixinReceivedEventStore<WeixinReceivedEventEntity> { }

/// <summary>
/// Provides an abstraction for storing information of received events from Weixin subscribers.
/// </summary>
/// <typeparam name="TWeixinReceivedEventEntity">The type that represents a received events from a Weixin subscriber.</typeparam>
public interface IWeixinReceivedEventStore<TWeixinReceivedEventEntity> : IEntityStore<TWeixinReceivedEventEntity>, IQueryableEntityStore<TWeixinReceivedEventEntity>
    where TWeixinReceivedEventEntity : class, IWeixinReceivedEvent, IEntity
{
    Task<int> GetCountByOpenIdAsync(string openId, CancellationToken cancellationToken = default);

    Task<IList<TWeixinReceivedEventEntity>> GetItemsByOpenIdAsync(string openId, int perPage, int pageIndex, CancellationToken cancellationToken = default);
}
