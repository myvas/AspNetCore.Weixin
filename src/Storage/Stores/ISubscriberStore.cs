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
    /// Creates the specified <paramref name="subscriber"/> in the <see cref="Subscriber"/> store.
    /// </summary>
    /// <param name="subscriber">The <see cref="Subscriber"/> to create.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="WeixinResult"/> of the creation operation.</returns>
    Task<WeixinResult> CreateAsync(TSubscriber subscriber, CancellationToken cancellationToken = default);

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
    Task<int> GetSubscribersCountAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="perPage"></param>
    /// <param name="pageIndex"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IList<TSubscriber>> GetSubscribersAsync(int perPage, int pageIndex, CancellationToken cancellationToken = default);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TSubscriber> FindByUserIdAsync(string userId, CancellationToken cancellationToken = default);


    /// <summary>
    /// Finds and returns a <see cref="Subscriber"/>, if any, who has the specified <paramref name="openId"/>.
    /// </summary>
    /// <param name="openId">The OpenId to search for.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>
    /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="Subscriber"/> matching the specified <paramref name="openId"/> if it exists.
    /// </returns>
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
    Task<TSubscriber> FindByNickNameAsync(string nickname, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the specified <paramref name="subscriber"/> in the <see cref="ISubscriberStore{TSubscriber}"/>.
    /// </summary>
    /// <param name="subscriber">The <see cref="Subscriber"/> to update.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchonous operation, containing the <see cref="WeixinResult"/> of the update operation.</returns>
    Task<WeixinResult> UpdateAsync(TSubscriber subscriber, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets the OpenId for a <see cref="Subscriber"/> from the store as an asynchronous operation.
    /// </summary>
    /// <param name="subscriber">The <see cref="Subscriber"/> whose OpenId should be returned.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>A <see cref="Task{TResult}"/> that contains the OpenId of the <see cref="Subscriber"/>.</returns>
    Task<string> GetOpenIdAsync(TSubscriber subscriber, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the UserId for a <see cref="Subscriber"/> from the store as an asynchronous operation.
    /// </summary>
    /// <param name="subscriber">The <see cref="Subscriber"/> whose UserId should be returned.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>A <see cref="Task{TResult}"/> that contains the UserId of the <see cref="Subscriber"/>.</returns>
    Task<string> GetUserIdAsync(TSubscriber subscriber, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets the AppId of a <see cref="Subscriber"/> in the store as an asynchronous operation.
    /// </summary>
    /// <param name="subscriber">The <see cref="Subscriber"/> whose AppId should be set.</param>
    /// <param name="appId">The AppId of the <see cref="Subscriber"/>.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    Task SetAppIdAsync(TSubscriber subscriber, string appId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets the UserId of a <see cref="Subscriber"/> in the store as an asynchronous operation.
    /// </summary>
    /// <param name="subscriber">The <see cref="Subscriber"/> whose UserId should be set.</param>
    /// <param name="userId">The UserId of the <see cref="Subscriber"/>.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    Task SetUserIdAsync(TSubscriber subscriber, string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets the MentorId of a <see cref="Subscriber"/> in the store as an asynchronous operation.
    /// </summary>
    /// <param name="subscriber">The <see cref="Subscriber"/> whose MentorId should be set.</param>
    /// <param name="mentorId">The MentorId of the <see cref="Subscriber"/>.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    Task SetMentorIdAsync(TSubscriber subscriber, string mentorId, CancellationToken cancellationToken = default);
}
