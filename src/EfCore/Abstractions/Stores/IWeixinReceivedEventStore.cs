using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace Myvas.AspNetCore.Weixin
{
    public interface IWeixinReceivedEventStore : IWeixinReceivedEventStore<WeixinReceivedEvent> { }

    /// <summary>
    /// Provides an abstraction for storing information of received events from Weixin subscribers.
    /// </summary>
    /// <typeparam name="TWeixinReceivedEvent">The type that represents a received events from a Weixin subscriber.</typeparam>
    public interface IWeixinReceivedEventStore<TWeixinReceivedEvent> : IEntityStore<TWeixinReceivedEvent>, IQueryableEntityStore<TWeixinReceivedEvent>
        where TWeixinReceivedEvent : class, IEntity
    {
        Task<int> GetCountByOpenIdAsync(string openId, CancellationToken cancellationToken = default);

        Task<IList<TWeixinReceivedEvent>> GetItemsByOpenIdAsync(string openId, int perPage, int pageIndex, CancellationToken cancellationToken = default);
    }
}
