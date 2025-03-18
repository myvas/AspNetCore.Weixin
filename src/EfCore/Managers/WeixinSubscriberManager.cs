using Microsoft.Extensions.Logging;
using System;
using System.Threading;

namespace Myvas.AspNetCore.Weixin.EfCore;

public class WeixinSubscriberManager<TWeixinSubscriber> : WeixinSubscriberManager<TWeixinSubscriber, string>
    where TWeixinSubscriber : WeixinSubscriber
{
    public WeixinSubscriberManager(IWeixinSubscriberStore<TWeixinSubscriber, string> store, WeixinErrorDescriber errors, ILogger<WeixinSubscriberManager<TWeixinSubscriber>> logger) : base(store, errors, logger)
    {
    }
}

public class WeixinSubscriberManager<TWeixinSubscriber, TKey> : IDisposable
where TWeixinSubscriber : WeixinSubscriber
where TKey : IEquatable<TKey>
{
    /// <summary>
    /// The cancellation token used to cancel operations.
    /// </summary>
    protected virtual CancellationToken CancellationToken => CancellationToken.None;

    protected internal IWeixinSubscriberStore<TWeixinSubscriber, TKey> Store { get; set; }
    public virtual ILogger Logger { get; set; }

    /// <summary>
    /// The <see cref="WeixinErrorDescriber"/> used to generate error messages.
    /// </summary>
    public WeixinErrorDescriber ErrorDescriber { get; set; }

    public WeixinSubscriberManager(IWeixinSubscriberStore<TWeixinSubscriber, TKey> store,
        WeixinErrorDescriber errors,
        ILogger<WeixinSubscriberManager<TWeixinSubscriber>> logger)
    {
        if (store == null)
        {
            throw new ArgumentNullException(nameof(store));
        }

        Store = store;
        ErrorDescriber = errors;
        Logger = logger;
    }

    #region IDispose
    private bool _disposed;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing && !_disposed)
        {
            Store.Dispose();
            _disposed = true;
        }
    }

    /// <summary>
    /// Throws if this class has been disposed.
    /// </summary>
    protected void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(GetType().Name);
        }
    }
    #endregion
}
