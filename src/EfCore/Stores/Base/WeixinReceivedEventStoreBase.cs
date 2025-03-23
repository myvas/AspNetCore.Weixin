using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public abstract class WeixinReceivedEventStoreBase<TWeixinReceivedEventEntity> : EntityStoreBase<TWeixinReceivedEventEntity>, IWeixinReceivedEventStore<TWeixinReceivedEventEntity>
    where TWeixinReceivedEventEntity : class, IWeixinReceivedEventEntity
{
    public WeixinReceivedEventStoreBase(WeixinErrorDescriber describer)
    {
        ErrorDescriber = describer ?? throw new ArgumentNullException(nameof(describer));
    }

    /// <summary>
    /// Gets or sets the <see cref="WeixinErrorDescriber"/> for any error that occurred with the current operation.
    /// </summary>
    public WeixinErrorDescriber ErrorDescriber { get; set; }

    public virtual Task<int> GetCountByOpenIdAsync(string openId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (openId == null)
        {
            throw new ArgumentNullException(nameof(openId));
        }
        var result = Items.Count(x => x.FromUserName == openId);
        return Task.FromResult(result);
    }


    public virtual Task<IList<TWeixinReceivedEventEntity>> GetItemsByOpenIdAsync(string openId, int perPage, int pageIndex, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (openId == null)
        {
            throw new ArgumentNullException(nameof(openId));
        }
        var result = Items.Where(x => x.FromUserName == openId);
        return Task.FromResult((IList<TWeixinReceivedEventEntity>)result);
    }
}
