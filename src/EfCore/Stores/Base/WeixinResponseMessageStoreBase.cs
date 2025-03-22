using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public abstract class WeixinResponseMessageStoreBase<TWeixinResponseMessageEntity> : EntityStoreBase<TWeixinResponseMessageEntity>, IWeixinResponseMessageStore<TWeixinResponseMessageEntity>
    where TWeixinResponseMessageEntity : class, IWeixinResponseMessageEntity
{
    public WeixinResponseMessageStoreBase(WeixinErrorDescriber describer)
    {
        ErrorDescriber = describer ?? throw new ArgumentNullException(nameof(describer));
    }

    /// <summary>
    /// Gets or sets the <see cref="WeixinErrorDescriber"/> for any error that occurred with the current operation.
    /// </summary>
    public WeixinErrorDescriber ErrorDescriber { get; set; }

    public virtual Task<int> GetCountByDestOpenIdAsync(string openId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (openId == null)
        {
            throw new ArgumentNullException(nameof(openId));
        }
        var result = Items.Count(x => x.ToUserName == openId);
        return Task.FromResult(result);
    }

    public virtual Task<IList<TWeixinResponseMessageEntity>> GetItemsByDestOpenIdAsync(string openId, int perPage, int pageIndex, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (openId == null)
        {
            throw new ArgumentNullException(nameof(openId));
        }
        var result = Items.Where(x => x.ToUserName == openId);
        return Task.FromResult((IList<TWeixinResponseMessageEntity>)result);
    }

    public virtual Task<IList<TWeixinResponseMessageEntity>> GetRequestMessagesAsync(string id, int perPage, int pageIndex, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
