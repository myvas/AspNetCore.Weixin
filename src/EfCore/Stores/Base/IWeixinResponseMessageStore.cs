using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace Myvas.AspNetCore.Weixin;

public interface IWeixinResponseMessageStore : IWeixinResponseMessageStore<WeixinResponseMessageEntity> { }

/// <summary>
/// Provides an abstraction for storing information of response messages related with Weixin received message.
/// </summary>
/// <typeparam name="TWeixinResponseMessageEntity">The type that represents a response message related with a Weixin received message.</typeparam>
public interface IWeixinResponseMessageStore<TWeixinResponseMessageEntity> : IEntityStore<TWeixinResponseMessageEntity>, IQueryableEntityStore<TWeixinResponseMessageEntity>
    where TWeixinResponseMessageEntity : class, IEntity
{
    Task<IList<TWeixinResponseMessageEntity>> GetRequestMessagesAsync(string id, int perPage, int pageIndex, CancellationToken cancellationToken = default);
    Task<int> GetCountByDestOpenIdAsync(string openId, CancellationToken cancellationToken = default);
    Task<IList<TWeixinResponseMessageEntity>> GetItemsByDestOpenIdAsync(string openId, int perPage, int pageIndex, CancellationToken cancellationToken = default);
}
