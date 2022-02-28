using Myvas.AspNetCore.Weixin.Models;

namespace Myvas.AspNetCore.Weixin.EntityFrameworkCore;

/// <summary>
/// Interface to model notifications from the TokenCleanup feature.
/// </summary>
public interface ISubscriptionNotification
{
    /// <summary>
    /// Notification for persisted tokens being removed.
    /// </summary>
    /// <param name="subscribers">The subscribers</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns></returns>
    Task SubscribeAsync(IEnumerable<WeixinSubscriber> subscribers, CancellationToken cancellationToken = default);

    /// <summary>
    /// Notification for persisted tokens being removed.
    /// </summary>
    /// <param name="subscribers">The subscribers</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns></returns>
    Task UnsubscribeAsync(IEnumerable<WeixinSubscriber> subscribers, CancellationToken cancellationToken = default);
}