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
public class WeixinSubscriberSyncService<TWeixinDbContext, TWeixinSubscriberEntity, TKey> : IWeixinSubscriberSyncService
    where TWeixinDbContext : DbContext, IWeixinDbContext<TWeixinSubscriberEntity, TKey>
    where TWeixinSubscriberEntity : class, IWeixinSubscriberEntity<TKey>, new()
    where TKey : IEquatable<TKey>
{
    private readonly IWeixinUserApi _api;
    private readonly DbContextOptions _options;
    private readonly DbContextFactory<TWeixinDbContext> _contextFactory;
    private readonly ILogger<WeixinSubscriberSyncService<TWeixinDbContext, TWeixinSubscriberEntity, TKey>> _logger;

    public WeixinSubscriberSyncService(
        IWeixinUserApi api,
        DbContextOptions options,
        DbContextFactory<TWeixinDbContext> contextFactory,
        ILogger<WeixinSubscriberSyncService<TWeixinDbContext, TWeixinSubscriberEntity, TKey>> logger)
    {
        _api = api ?? throw new ArgumentNullException(nameof(api));
        _options = options;
        _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
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

        using var context = _contextFactory.CreateDbContext(_options);

        foreach (var user in userInfos)
        {
            var subscriber = await context.WeixinSubscribers.FirstOrDefaultAsync(x => x.OpenId == (user.OpenId ?? ""), cancellationToken);
            if (subscriber == null)
            {
                msg = $"Find a new subscriber: {user.OpenId}";
                Debug.WriteLine(msg);
                _logger.LogTrace(msg);
                try
                {
                    var createResult = await context.AddAsync(new TWeixinSubscriberEntity
                    {
                        OpenId = user.OpenId,
                        HeadImgUrl = user.HeadImgUrl,
                        City = user.City,
                        Province = user.Province,
                        Country = user.Country,
                        Nickname = user.Nickname,
                        Sex = user.Sex,
                        Language = user.Language,
                        Remark = user.Remark,
                        UnionId = user.UnionId,
                        SubscribeTime = user.SubscribeTime,
                        Subscribed = user.SubscribeAsBool()
                    }, cancellationToken);
                    var saveResult = await context.SaveChangesAsync(cancellationToken);
                    if (saveResult == 1)
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
                catch (DbUpdateConcurrencyException cex)
                {
                    msg = $"An update concurrency exception occurred while creating a subscriber: {user.OpenId}";
                    Trace.WriteLine(msg);
                    _logger.LogError(cex, msg);
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
                    msg = $"An unexpected error occurred during database operations for creating a subscriber: {user.OpenId}";
                    Trace.WriteLine(msg);
                    Debug.WriteLine(ex);
                    _logger.LogError(ex, msg);
                }
            }
            else
            {
                bool dirty = false;
                if (subscriber.HeadImgUrl != user.HeadImgUrl) { dirty = true; subscriber.HeadImgUrl = user.HeadImgUrl; }
                if (subscriber.City != user.City) { dirty = true; subscriber.City = user.City; }
                if (subscriber.Province != user.Province) { dirty = true; subscriber.Province = user.Province; }
                if (subscriber.Country != user.Country) { dirty = true; subscriber.Country = user.Country; }
                if (subscriber.Nickname != user.Nickname) { dirty = true; subscriber.Nickname = user.Nickname; }
                if (subscriber.Sex != user.Sex) { dirty = true; subscriber.Sex = user.Sex; }
                if (subscriber.Language != user.Language) { dirty = true; subscriber.Language = user.Language; }
                if (subscriber.Remark != user.Remark) { dirty = true; subscriber.Remark = user.Remark; }
                if (subscriber.UnionId != user.UnionId) { dirty = true; subscriber.UnionId = user.UnionId; }
                if (subscriber.SubscribeTime != user.SubscribeTime) { dirty = true; subscriber.SubscribeTime = user.SubscribeTime; }
                if (subscriber.Subscribed != user.SubscribeAsBool()) { dirty = true; subscriber.Subscribed = user.SubscribeAsBool(); }

                if (dirty)
                {
                    try
                    {
                        var updateResult = context.Update(subscriber);
                        var saveResult = await context.SaveChangesAsync(cancellationToken);
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
        var totalSubscribedInDb = await context.WeixinSubscribers.CountAsync(x => x.Subscribed, cancellationToken);
        var totalUnsubscribedInDb = await context.WeixinSubscribers.CountAsync(x => !x.Subscribed, cancellationToken);
        Trace.WriteLine($"In db, subscribed: {totalSubscribedInDb}, unsubscribed: {totalUnsubscribedInDb}");
        msg = $"This round of pulling task is done. total: {totalCounter}, new: {createdCounter}, updated: {updatedCounter}";
        Trace.WriteLine(msg);
        _logger.LogInformation(msg);
    }
}
