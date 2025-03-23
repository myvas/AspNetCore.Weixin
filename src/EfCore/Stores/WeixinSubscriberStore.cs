using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin.EfCore;

public class WeixinSubscriberStore<TContext> : WeixinSubscriberStore<WeixinSubscriberEntity, TContext>, IWeixinSubscriberStore
    where TContext : DbContext
{
    public WeixinSubscriberStore(TContext context, WeixinErrorDescriber describer = null) : base(context, describer)
    {
    }
}

public class WeixinSubscriberStore<TWeixinSubscriberEntity, TContext> : WeixinSubscriberStore<TWeixinSubscriberEntity, string, TContext>, IWeixinSubscriberStore<TWeixinSubscriberEntity>
    where TWeixinSubscriberEntity : class, IWeixinSubscriberEntity<string>
    where TContext : DbContext
{
    public WeixinSubscriberStore(TContext context, WeixinErrorDescriber describer = null) : base(context, describer)
    {
    }
}

public class WeixinSubscriberStore<TWeixinSubscriberEntity, TKey, TContext> : WeixinSubscriberStoreBase<TWeixinSubscriberEntity, TKey>
    where TWeixinSubscriberEntity : class, IWeixinSubscriberEntity<TKey>
    where TKey : IEquatable<TKey>
    where TContext : DbContext
{
    public WeixinSubscriberStore(TContext context, WeixinErrorDescriber describer = null)
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

    public override IQueryable<TWeixinSubscriberEntity> Items => Context.Set<TWeixinSubscriberEntity>();

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
    protected Task SaveChanges(CancellationToken cancellationToken = default)
    {
        return AutoSaveChanges ? Context.SaveChangesAsync(cancellationToken) : Task.CompletedTask;
    }

    /// <summary>
    /// Creates a new role in a store as an asynchronous operation.
    /// </summary>
    /// <param name="role">The role to create in the store.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
    public override async Task<WeixinResult> CreateAsync(TWeixinSubscriberEntity item, CancellationToken cancellationToken = default)
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

    /// <summary>
    /// Updates a role in a store as an asynchronous operation.
    /// </summary>
    /// <param name="role">The role to update in the store.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
    public override async Task<WeixinResult> UpdateAsync(TWeixinSubscriberEntity item, CancellationToken cancellationToken = default)
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

    /// <summary>
    /// Deletes an item from the store as an asynchronous operation.
    /// </summary>
    /// <param name="item">The item to delete from the store.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="WeixinResult"/> of the asynchronous query.</returns>
    public override async Task<WeixinResult> DeleteAsync(TWeixinSubscriberEntity item, CancellationToken cancellationToken = default)
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

    /// <summary>
    /// Sets the name of a role in the store as an asynchronous operation.
    /// </summary>
    /// <param name="role">The role whose name should be set.</param>
    /// <param name="roleName">The name of the role.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public override Task<WeixinResult> SetUserIdAsync(TWeixinSubscriberEntity item, TKey userId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }
        item.UserId = userId;
        return UpdateAsync(item, cancellationToken);
    }

    public override Task<WeixinResult> AddAssociationAsync(TWeixinSubscriberEntity item, TKey userId, CancellationToken cancellationToken = default)
        => SetUserIdAsync(item, userId, cancellationToken);

    public override Task<WeixinResult> RemoveAssociationAsync(TWeixinSubscriberEntity item, CancellationToken cancellationToken = default)
        => SetUserIdAsync(item, default, cancellationToken);
}
