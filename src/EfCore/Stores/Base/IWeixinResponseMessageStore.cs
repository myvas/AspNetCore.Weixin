using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace Myvas.AspNetCore.Weixin;

public interface IWeixinResponseMessageStore : IWeixinResponseMessageStore<WeixinResponseMessage> { }

/// <summary>
/// Provides an abstraction for storing information of response messages related with Weixin received message.
/// </summary>
/// <typeparam name="TWeixinResponseMessage">The type that represents a response message related with a Weixin received message.</typeparam>
public interface IWeixinResponseMessageStore<TWeixinResponseMessage> : IEntityStore<TWeixinResponseMessage>, IQueryableEntityStore<TWeixinResponseMessage>
    where TWeixinResponseMessage : class, IEntity
{
    Task<IList<TWeixinResponseMessage>> GetRequestMessagesAsync(string id, int perPage, int pageIndex, CancellationToken cancellationToken = default);
    Task<int> GetCountByDestOpenIdAsync(string openId, CancellationToken cancellationToken = default);
    Task<IList<TWeixinResponseMessage>> GetItemsByDestOpenIdAsync(string openId, int perPage, int pageIndex, CancellationToken cancellationToken = default);
}
