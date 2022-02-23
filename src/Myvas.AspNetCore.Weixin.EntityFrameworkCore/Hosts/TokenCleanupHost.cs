using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Myvas.AspNetCore.Weixin.AccessTokenServer.EntityFrameworkCore;
using Myvas.AspNetCore.Weixin.AccessTokenServer.EntityFrameworkCore.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin.AccessTokenServer;

public class TokenCleanupHost : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly OperationalStoreOptions _options;
    private readonly ILogger<TokenCleanupHost> _logger;

    private TimeSpan CleanupInterval => TimeSpan.FromSeconds(_options.TokenCleanupInterval);

    private CancellationTokenSource _source;

    public TokenCleanupHost(IServiceProvider serviceProvider, OperationalStoreOptions options, ILogger<TokenCleanupHost> logger)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        if (_options.EnableTokenCleanup)
        {
            if (_source != null) throw new InvalidOperationException("Already started. Call stop first.");

            _logger.LogDebug("Starting token removal");

            _source = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            Task.Factory.StartNew(() => StartInternalAsync(_source.Token));
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        if (_options.EnableTokenCleanup)
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
                await Task.Delay(CleanupInterval, cancellationToken);
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

            await RemoveExpiredTokensAsync(cancellationToken);
        }
    }

    async Task RemoveExpiredTokensAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            using (var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var tokenCleanupService = serviceScope.ServiceProvider.GetRequiredService<TokenCleanupService>();
                await tokenCleanupService.RemoveExpiredTokensAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception removing expired tokens: {exception}", ex.Message);
        }
    }
}