using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Myvas.AspNetCore.Weixin;

public interface IWeixinSubscriberStore : IWeixinSubscriberStore<WeixinSubscriber, string> { }

/// <summary>
/// Provides an abstraction for storing information of Weixin subscribers (and its related IdentityUser).
/// </summary>
/// <typeparam name="TWeixinSubscriber">The type that represents a Weixin subscriber.</typeparam>
public interface IWeixinSubscriberStore<TWeixinSubscriber, TKey> : IEntityStore<TWeixinSubscriber>, IQueryableEntityStore<TWeixinSubscriber>
    where TWeixinSubscriber : class, IEntity
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Adds a relationship between the specified <paramref name="subscriber"/> to an external IdentityUser via <paramref name="userId"/>.
    /// </summary>
    /// <param name="subscriber">The WeixinSubsciber to add the relationship to.</param>
    /// <param name="userId">The IdentityUser.Id to add to the relationship to.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    Task<WeixinResult> AddAssociationAsync(TWeixinSubscriber subscriber, TKey userId, CancellationToken cancellationToken = default);
    Task<WeixinResult> RemoveAssociationAsync(TWeixinSubscriber subscriber, CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds the entity who has the specified ID as an asynchronous operation.
    /// </summary>
    /// <param name="id">The entity ID to look for.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>A <see cref="Task{TResult}"/> that result of the look up.</returns>
    Task<TWeixinSubscriber> FindByOpenIdAsync(string openId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds the entity who has the specified ID as an asynchronous operation.
    /// </summary>
    /// <param name="id">The entity ID to look for.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>A <see cref="Task{TResult}"/> that result of the look up.</returns>
    Task<TWeixinSubscriber> FindByUserIdAsync(TKey userId, CancellationToken cancellationToken = default);

    Task<IList<TWeixinSubscriber>> GetItemsByMentorIdAsync(TKey mentorId, int perPage, int pageIndex, CancellationToken cancellationToken = default);
}
