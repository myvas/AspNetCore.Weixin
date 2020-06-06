using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Myvas.AspNetCore.Weixin
{
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
        Task RemoveSubscriberAsync(TWeixinSubscriber subscriber, string userId, CancellationToken cancellationToken);

        Task<int> GetSubscribersCountAsync();
        Task<IList<TWeixinSubscriber>> GetSubscribersAsync(int perPage, int pageIndex, CancellationToken cancellationToken);

        Task<TWeixinSubscriber> FindByIdAsync(string id, CancellationToken cancellationToken);
        Task<TWeixinSubscriber> FindByUserIdAsync(string userId, CancellationToken cancellationToken);
        Task<TWeixinSubscriber> FindByOpenIdAsync(string openId, CancellationToken cancellationToken);
        Task<TWeixinSubscriber> FindByUnionIdAsync(string unionId, CancellationToken cancellationToken);
        Task<TWeixinSubscriber> FindByNicknameAsync(string nickname, CancellationToken cancellationToken);
    }
}
