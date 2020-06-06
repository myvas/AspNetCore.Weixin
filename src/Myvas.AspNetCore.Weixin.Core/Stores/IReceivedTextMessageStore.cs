using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// Provides an abstraction for storing information of Weixin subscribers (and its related IdentityUser).
    /// </summary>
    /// <typeparam name="TWeixinSubscriber">The type that represents a Weixin subscriber.</typeparam>
    public interface IReceivedTextMessageStore<TReceivedTextMessage>
        where TReceivedTextMessage : class
    {
        /// <summary>
        /// Adds a relationship between the specified <paramref name="subscriber"/> to an external IdentityUser via <paramref name="userId"/>.
        /// </summary>
        /// <param name="subscriber">The WeixinSubsciber to add the relationship to.</param>
        /// <param name="userId">The IdentityUser.Id to add to the relationship to.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        Task AddTextMessageAsync(TReceivedTextMessage message, CancellationToken cancellationToken);
        Task RemoveTextMessageAsync(TReceivedTextMessage message, CancellationToken cancellationToken);

        Task<int> GetReceivedTextMessageCountAsync();
        Task<int> GetReceivedTextMessageCountByUserIdAsync();
        Task<int> GetReceivedTextMessageCountByOpenIdAsync();
        Task<int> GetReceivedTextMessageCountByUnionIdAsync();
        Task<int> GetReceivedTextMessageCountByNicknameAsync();
        Task<IList<TReceivedTextMessage>> GetReceivedTextMessagesAsync(int perPage, int pageIndex, CancellationToken cancellationToken);
        Task<IList<TReceivedTextMessage>> GetReceivedTextMessagesByUserIdAsync(string userId, int perPage, int pageIndex, CancellationToken cancellationToken);
        Task<IList<TReceivedTextMessage>> GetReceivedTextMessagesByOpenIdAsync(string openId, int perPage, int pageIndex, CancellationToken cancellationToken);
        Task<IList<TReceivedTextMessage>> GetReceivedTextMessagesByUnionIdAsync(string unionId, int perPage, int pageIndex, CancellationToken cancellationToken);
        Task<IList<TReceivedTextMessage>> GetReceivedTextMessagesByNicknameAsync(string nickname, int perPage, int pageIndex, CancellationToken cancellationToken);

        Task<TReceivedTextMessage> FindByIdAsync(string id, CancellationToken cancellationToken);
    }
}
