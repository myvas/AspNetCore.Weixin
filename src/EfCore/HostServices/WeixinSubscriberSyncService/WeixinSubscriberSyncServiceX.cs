using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Myvas.AspNetCore.Weixin.EfCore;

/// <summary>
/// Sync service to pull <see cref="IWeixinSubscriberEntity{TKey}"/> from Tencent server.
/// </summary>
public class WeixinSubscriberSyncServiceX<TWeixinSubscriberEntity, TKey> : IWeixinSubscriberSyncService
    where TWeixinSubscriberEntity : class, IWeixinSubscriberEntity<TKey>, IEntity, new()
    where TKey : IEquatable<TKey>
{
    private readonly IWeixinUserApi _api;
    private readonly IWeixinSubscriberStore<TWeixinSubscriberEntity, TKey> _store;
    private readonly ILogger<WeixinSubscriberSyncServiceX<TWeixinSubscriberEntity, TKey>> _logger;

    public WeixinSubscriberSyncServiceX(
        IWeixinUserApi api,
        IWeixinSubscriberStore<TWeixinSubscriberEntity, TKey> store,
        ILogger<WeixinSubscriberSyncServiceX<TWeixinSubscriberEntity, TKey>> logger)
    {
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
        var msg = "Start a task to fetch subscribers from Tencent server...";
        Trace.WriteLine(msg);
        _logger.LogInformation(msg);

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
            msg = "An error occurred while fetching data from the API.";
            Trace.WriteLine(msg);
            Debug.WriteLine(ex);
            _logger.LogError(ex, msg);
            return; // Exit the method if the API call fails
        }

        foreach (var user in userInfos)
        {
            var subscriber = await _store.Items.FirstOrDefaultAsync(x => x.OpenId == user.OpenId, cancellationToken);
            if (subscriber == null)
            {
                msg = $"Find a new subscriber: {user.OpenId}";
                Debug.WriteLine(msg);
                _logger.LogTrace(msg);
                try
                {
                    var createResult = await _store.CreateAsync(new TWeixinSubscriberEntity
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
                    if (createResult.Succeeded)
                    {
                        createdCounter++;

                        msg = $"Stored a new subscriber: {user.OpenId}";
                        Debug.WriteLine(msg);
                        _logger.LogTrace(msg);
                    }
                    else
                    {
                        msg = $"Found a new subscriber, but failed to store: {user.OpenId}";
                        Debug.WriteLine(msg);
                        _logger.LogTrace(msg);
                    }
                }
                catch (DbUpdateException uex)
                {
                    msg = $"An error occurred while creating a subscriber: {user.OpenId}";
                    Trace.WriteLine(msg);
                    _logger.LogError(uex, msg);
                }
                catch (Exception ex)
                {
                    // Handle other database-related exceptions
                    msg = $"An unexpected error occurred during database operations for subscriber: {user.OpenId}";
                    Trace.WriteLine(msg);
                    _logger.LogError(ex, msg);
                }
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

                        msg = $"Update a subscriber: {user.OpenId}";
                        Debug.WriteLine(msg);
                        _logger.LogTrace(msg);

                    }
                    catch (DbUpdateException uex)
                    {
                        msg = $"An error occurred while updating a subscriber: {user.OpenId}";
                        Trace.WriteLine(msg);
                        _logger.LogError(uex, msg);
                    }
                    catch (Exception ex)
                    {
                        // Handle other database-related exceptions
                        msg = $"An unexpected error occurred during database operations for subscriber: {user.OpenId}";
                        Trace.WriteLine(msg);
                        _logger.LogError(ex, msg);
                    }
                }
            }
        }

        var totalSubscribedInDb = await _store.Items.CountAsync(x => x.Subscribed, cancellationToken);
        var totalUnsubscribedInDb = await _store.Items.CountAsync(x => !x.Subscribed, cancellationToken);
        Trace.WriteLine($"In db {_store.GetHashCode()}, subscribed: {totalSubscribedInDb}, unsubscribed: {totalUnsubscribedInDb}");
        msg = $"This round of pulling task is done. total: {totalCounter}, new: {createdCounter}, updated: {updatedCounter}";
        Trace.WriteLine(msg);
        _logger.LogInformation(msg);
    }
}
