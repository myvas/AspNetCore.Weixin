using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Myvas.AspNetCore.Weixin.Models;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// Provides an abstraction for storing information of <see cref="Subscriber"/>.
/// </summary>
public interface ISubscriberStore : ISubscriberStore<Subscriber>
{
}

/// <summary>
/// Provides an abstraction for storing information of Weixin subscribers (and its related IdentityUser).
/// </summary>
/// <typeparam name="TSubscriber">The type that represents a Weixin subscriber.</typeparam>
public interface ISubscriberStore<TSubscriber> : IDisposable
    where TSubscriber : Subscriber
{
    /// <summary>
    /// Adds a relationship between the specified <paramref name="subscriber"/> to an external IdentityUser via <paramref name="userId"/>.
    /// </summary>
    /// <param name="subscriber">The WeixinSubsciber to add the relationship to.</param>
    /// <param name="userId">The IdentityUser.Id to add to the relationship to.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    Task AddSubscriberAsync(TSubscriber subscriber, string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="subscriber"></param>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task RemoveSubscriberAsync(TSubscriber subscriber, string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<int> GetSubscribersCountAsync();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="perPage"></param>
    /// <param name="pageIndex"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IList<TSubscriber>> GetSubscribersAsync(int perPage, int pageIndex, CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds and returns a user, if any, who has the specified <paramref name="id"/>.
    /// </summary>
    /// <param name="id">The user ID to search for.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>
    /// The <see cref="Task"/> that represents the asynchronous operation, containing the user matching the specified <paramref name="id"/> if it exists.
    /// </returns>
    Task<TSubscriber> FindByIdAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TSubscriber> FindByUserIdAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="openId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TSubscriber> FindByOpenIdAsync(string openId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="unionId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TSubscriber> FindByUnionIdAsync(string unionId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="nickname"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TSubscriber> FindByNicknameAsync(string nickname, CancellationToken cancellationToken = default);
}
