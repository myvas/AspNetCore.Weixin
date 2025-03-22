using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin.EfCore;

public class WeixinReceivedMessageStore<TContext> : WeixinReceivedMessageStore<WeixinReceivedMessageEntity, TContext>, IWeixinReceivedMessageStore
    where TContext : DbContext
{
    public WeixinReceivedMessageStore(TContext context, WeixinErrorDescriber describer = null) : base(context, describer)
    {
    }
}

public class WeixinReceivedMessageStore<TWeixinReceivedMessageEntity, TContext> : WeixinReceivedMessageStoreBase<TWeixinReceivedMessageEntity>
    where TWeixinReceivedMessageEntity : class, IWeixinReceivedMessageEntity
    where TContext : DbContext
{
    public WeixinReceivedMessageStore(TContext context, WeixinErrorDescriber describer = null)
        : base(describer ?? new WeixinErrorDescriber())
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }
        Context = context;
    }


    /// <summary>
    /// Gets the database context for this store.
    /// </summary>
    public virtual TContext Context { get; private set; }

    public override IQueryable<TWeixinReceivedMessageEntity> Items => Context.Set<TWeixinReceivedMessageEntity>();

    /// <summary>
    /// Gets or sets a flag indicating if changes should be persisted after CreateAsync, UpdateAsync and DeleteAsync are called.
    /// </summary>
    /// <value>
    /// True if changes should be automatically persisted, otherwise false.
    /// </value>
    public bool AutoSaveChanges { get; set; } = true;

    /// <summary>Saves the current store.</summary>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    protected Task SaveChanges(CancellationToken cancellationToken)
    {
        return AutoSaveChanges ? Context.SaveChangesAsync(cancellationToken) : Task.CompletedTask;
    }

    public override async Task<WeixinResult> CreateAsync(TWeixinReceivedMessageEntity item, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }
        Context.Add(item);
        await SaveChanges(cancellationToken);
        return WeixinResult.Success;
    }

    public override async Task<WeixinResult> UpdateAsync(TWeixinReceivedMessageEntity item, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        Context.Attach(item);
        item.ConcurrencyStamp = Guid.NewGuid().ToString();
        Context.Update(item);
        try
        {
            await SaveChanges(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            return WeixinResult.Failed(ErrorDescriber.ConcurrencyFailure());
        }
        return WeixinResult.Success;
    }

    public override async Task<WeixinResult> DeleteAsync(TWeixinReceivedMessageEntity item, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        Context.Remove(item);
        try
        {
            await SaveChanges(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            return WeixinResult.Failed(ErrorDescriber.ConcurrencyFailure());
        }
        return WeixinResult.Success;
    }
}
