using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Myvas.AspNetCore.Weixin.EfCore;

/// <summary>
/// Sync service to pull <see cref="WeixinSubscriber"/> from Tencent server.
/// </summary>
public class WeixinSubscriberSyncService<TWeixinSubscriberEntity, TKey>
    where TWeixinSubscriberEntity : class, IWeixinSubscriber<TKey>, IEntity, new()
    where TKey : IEquatable<TKey>
{
    private readonly WeixinSiteEfCoreOptions _options;
    private readonly IWeixinUserApi _api;
    private readonly IWeixinSubscriberStore<TWeixinSubscriberEntity, TKey> _store;
    private readonly ILogger<WeixinSubscriberSyncService<TWeixinSubscriberEntity, TKey>> _logger;

    public WeixinSubscriberSyncService(
        WeixinSiteEfCoreOptions options,
        IWeixinUserApi api,
        IWeixinSubscriberStore<TWeixinSubscriberEntity, TKey> store,
        ILogger<WeixinSubscriberSyncService<TWeixinSubscriberEntity, TKey>> logger)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _api = api ?? throw new ArgumentNullException(nameof(api));
        _store = store ?? throw new ArgumentNullException(nameof(store));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Pull subscribers from Tencent server.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    public async Task PullSubscribersAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start a task to fetch subscribers from Tencent server...");

        int createdCounter = 0;
        int updatedCounter = 0;
        int totalCounter = 0;
        List<WeixinUserInfoJson> userInfos = null;

        try
        {
            userInfos = await _api.GetAllUserInfo(cancellationToken);
            totalCounter = userInfos.Count;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching data from the API.");
            return; // Exit the method if the API call fails
        }

        foreach (var user in userInfos)
        {
            var subscriber = await _store.Items.FirstOrDefaultAsync(x => x.OpenId == user.OpenId);
            if (subscriber == null)
            {
                _logger.LogTrace("Find a new subscriber: {openId}", user.OpenId);
                await _store.CreateAsync(new TWeixinSubscriberEntity
                {
                    OpenId = user.OpenId,
                    AvatorImageUrl = user.AvatorImageUrl,
                    City = user.City,
                    Province = user.Province,
                    Country = user.Country,
                    Nickname = user.Nickname,
                    Gender = user.Gender,
                    Language = user.Language,
                    Remark = user.Remark,
                    UnionId = user.UnionId,
                    SubscribedTime = user.SubscribedTime,
                    Subscribed = user.SubscribeAsBool()
                }, cancellationToken);
                createdCounter++;
            }
            else
            {
                bool dirty = false;
                if (subscriber.AvatorImageUrl != user.AvatorImageUrl) { dirty = true; subscriber.AvatorImageUrl = user.AvatorImageUrl; }
                if (subscriber.City != user.City) { dirty = true; subscriber.City = user.City; }
                if (subscriber.Province != user.Province) { dirty = true; subscriber.Province = user.Province; }
                if (subscriber.Country != user.Country) { dirty = true; subscriber.Country = user.Country; }
                if (subscriber.Nickname != user.Nickname) { dirty = true; subscriber.Nickname = user.Nickname; }
                if (subscriber.Gender != user.Gender) { dirty = true; subscriber.Gender = user.Gender; }
                if (subscriber.Language != user.Language) { dirty = true; subscriber.Language = user.Language; }
                if (subscriber.Remark != user.Remark) { dirty = true; subscriber.Remark = user.Remark; }
                if (subscriber.UnionId != user.UnionId) { dirty = true; subscriber.UnionId = user.UnionId; }
                if (subscriber.SubscribedTime != user.SubscribedTime) { dirty = true; subscriber.SubscribedTime = user.SubscribedTime; }
                if (subscriber.Subscribed != user.SubscribeAsBool()) { dirty = true; subscriber.Subscribed = user.SubscribeAsBool(); }

                if (dirty)
                {
                    try
                    {
                        await _store.UpdateAsync(subscriber, cancellationToken);
                        updatedCounter++;
                        _logger.LogTrace("Update a subscriber: {openId}", user.OpenId);

                    }
                    catch (DbUpdateException uex)
                    {
                        _logger.LogError(uex, "An error occurred while updating a subscriber: {openId}", user.OpenId);
                    }
                    catch (Exception ex)
                    {
                        // Handle other database-related exceptions
                        _logger.LogError(ex, "An unexpected error occurred during database operations for subscriber: {openId}", user.OpenId);
                    }
                }
            }
        }

        _logger.LogInformation("This round of pulling task is done. total: {total}, new: {created}, updated: {updated}", totalCounter, createdCounter, updatedCounter);
    }
}
