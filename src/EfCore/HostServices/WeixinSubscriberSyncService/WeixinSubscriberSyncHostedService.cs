using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin.EfCore;

/// <summary>
/// A IHostService for sync service for <see cref="TWeixinSubscriberEntity"/>.
/// </summary>
public class WeixinSubscriberSyncHostedService : IHostedService, IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly WeixinSiteEfCoreOptions _options;
    private readonly ILogger<WeixinSubscriberSyncHostedService> _logger;

    private bool EnableSync;
    private TimeSpan SyncInterval;
    private CancellationTokenSource _cancellationTokenSource;

    public WeixinSubscriberSyncHostedService(IServiceProvider serviceProvider, IOptions<WeixinSiteEfCoreOptions> optionsAccessor, ILogger<WeixinSubscriberSyncHostedService> logger)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _options = optionsAccessor?.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));
        _logger = logger;

        EnableSync = _options.EnableSyncForWeixinSubscribers;

        if (_options.SyncIntervalInMinutesForWeixinSubscribers < WeixinSiteEfCoreOptions.MinSyncIntervalInMinutesForWeixinSubscribers)
            _options.SyncIntervalInMinutesForWeixinSubscribers = WeixinSiteEfCoreOptions.MinSyncIntervalInMinutesForWeixinSubscribers;
        SyncInterval = TimeSpan.FromMinutes(_options.SyncIntervalInMinutesForWeixinSubscribers);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        if (EnableSync)
        {
            var msg = "Starting sync service for Weixin subscribers.";
            Trace.WriteLine(msg);
            _logger.LogDebug(msg);

            if (_cancellationTokenSource != null)
            {
                var msg2 = "Already started. Call stop first.";
                Trace.WriteLine(msg2);
                throw new InvalidOperationException(msg2);
            }
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            
            Task.Factory.StartNew(() => StartInternalAsync(_cancellationTokenSource.Token));
        }
        else
        {
            Trace.WriteLine("The WeixinSubscriberSyncHostedService is not enabled.");
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        if (EnableSync)
        {
            if (_cancellationTokenSource == null)
            {
                var msg2 = "Not started. Call start first.";
                Debug.WriteLine(msg2);
                throw new InvalidOperationException(msg2);
            }

            var msg = "Stopping sync service for Weixin subscribers.";
            Trace.WriteLine(msg);
            _logger.LogDebug(msg);

            _cancellationTokenSource.Cancel();
            _cancellationTokenSource = null;
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

            await PullWeixinSubscribersAsync(cancellationToken);

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
            using var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var pullingService = serviceScope.ServiceProvider.GetRequiredService<IWeixinSubscriberSyncService>();
            await pullingService.PullSubscribersAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            Trace.WriteLine("An exception threw out while pulling subscribers.");
            Debug.WriteLine(ex);
            _logger.LogError("Exception pulling subscribers: {exception}", ex.Message);
        }
    }

    public void Dispose()
    {
    }
}