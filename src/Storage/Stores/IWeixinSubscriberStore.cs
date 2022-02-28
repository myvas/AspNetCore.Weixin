using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// Provides an abstraction for storing information of Weixin subscribers (and its related IdentityUser).
/// </summary>
/// <typeparam name="TWeixinSubscriber">The type that represents a Weixin subscriber.</typeparam>
public interface IWeixinSubscriberStore<TWeixinSubscriber> : IDisposable
    where TWeixinSubscriber : class
{
    /// <summary>
    /// Adds a relationship between the specified <paramref name="subscriber"/> to an external IdentityUser via <paramref name="userId"/>.
    /// </summary>
    /// <param name="subscriber">The WeixinSubsciber to add the relationship to.</param>
    /// <param name="userId">The IdentityUser.Id to add to the relationship to.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    Task AddSubscriberAsync(TWeixinSubscriber subscriber, string userId, CancellationToken cancellationToken);

    /// <inheritdoc/>
    Task RemoveSubscriberAsync(TWeixinSubscriber subscriber, string userId, CancellationToken cancellationToken);

    /// <inheritdoc/>
    Task<int> GetSubscribersCountAsync();

    /// <inheritdoc/>
    Task<IList<TWeixinSubscriber>> GetSubscribersAsync(int perPage, int pageIndex, CancellationToken cancellationToken);

    /// <summary>
    /// Finds and returns a user, if any, who has the specified <paramref name="id"/>.
    /// </summary>
    /// <param name="id">The user ID to search for.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>
    /// The <see cref="Task"/> that represents the asynchronous operation, containing the user matching the specified <paramref name="id"/> if it exists.
    /// </returns>
    Task<TWeixinSubscriber> FindByIdAsync(string id, CancellationToken cancellationToken);

    /// <inheritdoc/>
    Task<TWeixinSubscriber> FindByUserIdAsync(string userId, CancellationToken cancellationToken);

    /// <inheritdoc/>
    Task<TWeixinSubscriber> FindByOpenIdAsync(string openId, CancellationToken cancellationToken);

    /// <inheritdoc/>
    Task<TWeixinSubscriber> FindByUnionIdAsync(string unionId, CancellationToken cancellationToken);

    /// <inheritdoc/>
    Task<TWeixinSubscriber> FindByNicknameAsync(string nickname, CancellationToken cancellationToken);
}
