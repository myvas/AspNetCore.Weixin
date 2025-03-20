using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin.EfCore;

/// <summary>
/// A IHostService for sync service for <see cref="TWeixinSubscriberEntity"/>.
/// </summary>
public class WeixinSubscriberSyncHostedService<TWeixinSubscriberEntity, TKey> : IHostedService
    where TWeixinSubscriberEntity : class, IWeixinSubscriber<TKey>, IEntity, new()
    where TKey : IEquatable<TKey>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly WeixinSiteEfCoreOptions _options;
    private readonly ILogger<WeixinSubscriberSyncHostedService<TWeixinSubscriberEntity, TKey>> _logger;

    private bool EnableSync => _options.EnableSyncForWeixinSubscribers;
    private TimeSpan SyncInterval => TimeSpan.FromSeconds(_options.SyncIntervalInMinutesForWeixinSubscribers < WeixinSiteEfCoreOptions.MinSyncIntervalInMinutesForWeixinSubscribers ? WeixinSiteEfCoreOptions.MinSyncIntervalInMinutesForWeixinSubscribers : _options.SyncIntervalInMinutesForWeixinSubscribers);

    private CancellationTokenSource _source;

    public WeixinSubscriberSyncHostedService(IServiceProvider serviceProvider, IOptions<WeixinSiteEfCoreOptions> optionsAccessor, ILogger<WeixinSubscriberSyncHostedService<TWeixinSubscriberEntity, TKey>> logger)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _options = optionsAccessor?.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        if (EnableSync)
        {
            if (_source != null) throw new InvalidOperationException("Already started. Call stop first.");

            _logger.LogDebug("Starting sync service for Weixin subscribers.");

            _source = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            Task.Factory.StartNew(() => StartInternalAsync(_source.Token));
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        if (EnableSync)
        {
            if (_source == null) throw new InvalidOperationException("Not started. Call start first.");

            _logger.LogDebug("Stopping token removal");

            _source.Cancel();
            _source = null;
        }

        return Task.CompletedTask;
    }

    private async Task StartInternalAsync(CancellationToken cancellationToken)
    {
        while (true)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogDebug("CancellationRequested. Exiting...");
                break;
            }

            try
            {
                await Task.Delay(SyncInterval, cancellationToken);
            }
            catch (TaskCanceledException)
            {
                _logger.LogDebug("TaskCanceledException. Exiting...");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError("Task.Delay exception: {0}. Exiting...", ex.Message);
                break;
            }

            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogDebug("CancellationRequested. Exiting...");
                break;
            }

            await PullWeixinSubscribersAsync(cancellationToken);
        }
    }

    /// <summary>
    /// Fetch and merge the <see cref="WeixinSubscriber"/>.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    async Task PullWeixinSubscribersAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            using (var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var pullingService = serviceScope.ServiceProvider.GetRequiredService<WeixinSubscriberSyncService<TWeixinSubscriberEntity, TKey>>();
                await pullingService.PullSubscribersAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception pulling subscribers: {exception}", ex.Message);
        }
    }
}