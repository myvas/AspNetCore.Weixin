using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Myvas.AspNetCore.Weixin;

public interface IWeixinSubscriberStore : IWeixinSubscriberStore<WeixinSubscriberEntity>
{

}

public interface IWeixinSubscriberStore<TWeixinSubscriberEntity> : IWeixinSubscriberStore<TWeixinSubscriberEntity, string>
    where TWeixinSubscriberEntity : class, IWeixinSubscriberEntity<string>
{

}

/// <summary>
/// Provides an abstraction for storing information of Weixin subscribers (and its related IdentityUser).
/// </summary>
/// <typeparam name="TWeixinSubscriberEntity">The type that represents a Weixin subscriber.</typeparam>
public interface IWeixinSubscriberStore<TWeixinSubscriberEntity, TKey> : IEntityStore<TWeixinSubscriberEntity>, IQueryableEntityStore<TWeixinSubscriberEntity>
    where TWeixinSubscriberEntity : class, IWeixinSubscriberEntity<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Adds a relationship between the specified <paramref name="subscriber"/> to an external IdentityUser via <paramref name="userId"/>.
    /// </summary>
    /// <param name="subscriber">The WeixinSubsciber to add the relationship to.</param>
    /// <param name="userId">The IdentityUser.Id to add to the relationship to.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    Task<WeixinResult> AddAssociationAsync(TWeixinSubscriberEntity subscriber, TKey userId, CancellationToken cancellationToken = default);
    Task<WeixinResult> RemoveAssociationAsync(TWeixinSubscriberEntity subscriber, CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds the entity who has the specified ID as an asynchronous operation.
    /// </summary>
    /// <param name="id">The entity ID to look for.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>A <see cref="Task{TResult}"/> that result of the look up.</returns>
    Task<TWeixinSubscriberEntity> FindByOpenIdAsync(string openId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds the entity who has the specified ID as an asynchronous operation.
    /// </summary>
    /// <param name="id">The entity ID to look for.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>A <see cref="Task{TResult}"/> that result of the look up.</returns>
    Task<TWeixinSubscriberEntity> FindByUserIdAsync(TKey userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Set the MentorId field with the UserId of mentor.
    /// </summary>
    /// <param name="subscriber"></param>
    /// <param name="mentorId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeixinResult> SetMentorIdAsync(TWeixinSubscriberEntity subscriber, TKey mentorId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all subscribers related to a MentorId
    /// </summary>
    /// <param name="mentorId"></param>
    /// <param name="perPage"></param>
    /// <param name="pageIndex"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IList<TWeixinSubscriberEntity>> GetItemsByMentorIdAsync(TKey mentorId, int perPage, int pageIndex, CancellationToken cancellationToken = default);
}
