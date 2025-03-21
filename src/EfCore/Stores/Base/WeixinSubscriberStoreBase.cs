using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public abstract class WeixinSubscriberStoreBase<TWeixinSubscriberEntity, TKey> : EntityStoreBase<TWeixinSubscriberEntity>, IWeixinSubscriberStore<TWeixinSubscriberEntity, TKey>
    where TWeixinSubscriberEntity : class, IWeixinSubscriber<TKey>, IEntity
    where TKey : IEquatable<TKey>
{
    public WeixinSubscriberStoreBase(WeixinErrorDescriber describer)
    {
        if (describer == null)
        {
            throw new ArgumentNullException(nameof(describer));
        }
        ErrorDescriber = describer;
    }

    /// <summary>
    /// Gets or sets the <see cref="WeixinErrorDescriber"/> for any error that occurred with the current operation.
    /// </summary>
    public WeixinErrorDescriber ErrorDescriber { get; set; }

    public virtual Task<WeixinResult> SetUserIdAsync(TWeixinSubscriberEntity subscriber, TKey userId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (subscriber == null)
        {
            throw new ArgumentNullException(nameof(subscriber));
        }
        subscriber.UserId = userId;
        return Task.FromResult(WeixinResult.Success);
    }

    public virtual Task<WeixinResult> SetMentorIdAsync(TWeixinSubscriberEntity subscriber, TKey mentorId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (subscriber == null)
        {
            throw new ArgumentNullException(nameof(subscriber));
        }
        subscriber.MentorId = mentorId;
        return Task.FromResult(WeixinResult.Success);
    }

    public virtual Task<WeixinResult> AddAssociationAsync(TWeixinSubscriberEntity subscriber, TKey userId, CancellationToken cancellationToken = default)
    {
        return SetUserIdAsync(subscriber, userId, cancellationToken);
    }

    public virtual Task<WeixinResult> RemoveAssociationAsync(TWeixinSubscriberEntity subscriber, CancellationToken cancellationToken = default)
    {
        return SetUserIdAsync(subscriber, default, cancellationToken);
    }

    public virtual Task<TWeixinSubscriberEntity> FindByUserIdAsync(TKey userId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (userId == null)
        {
            throw new ArgumentNullException(nameof(userId));
        }
        var item = Items.FirstOrDefault(x => userId.Equals(x.UserId));
        return Task.FromResult(item);
    }

    public virtual Task<TWeixinSubscriberEntity> FindByOpenIdAsync(string openId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (openId == null)
        {
            throw new ArgumentNullException(nameof(openId));
        }
        var item = Items.FirstOrDefault(x => openId.Equals(x.OpenId));
        return Task.FromResult(item);
    }


    public virtual Task<IList<TWeixinSubscriberEntity>> GetItemsByMentorIdAsync(TKey mentorId, int perPage, int pageIndex, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (mentorId == null)
        {
            throw new ArgumentNullException(nameof(mentorId));
        }
        var item = Items.Where(x => mentorId.Equals(x.MentorId)).ToList();
        return Task.FromResult((IList<TWeixinSubscriberEntity>)item);
    }

    #region TKey to string
    /// <summary>
    /// Converts the provided <paramref name="id"/> to a strongly typed key object.
    /// </summary>
    /// <param name="id">The id to convert.</param>
    /// <returns>An instance of <typeparamref name="TKey"/> representing the provided <paramref name="id"/>.</returns>
    public virtual TKey ConvertIdFromString(string id)
    {
        if (id == null)
        {
            return default(TKey);
        }
        return (TKey)TypeDescriptor.GetConverter(typeof(TKey)).ConvertFromInvariantString(id);
    }

    /// <summary>
    /// Converts the provided <paramref name="id"/> to its string representation.
    /// </summary>
    /// <param name="id">The id to convert.</param>
    /// <returns>An <see cref="string"/> representation of the provided <paramref name="id"/>.</returns>
    public virtual string ConvertIdToString(TKey id)
    {
        if (object.Equals(id, default(TKey)))
        {
            return null;
        }
        return id.ToString();
    }
    #endregion
}
