using Microsoft.EntityFrameworkCore;
using Myvas.AspNetCore.Weixin.Models;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.Stores;

namespace Myvas.AspNetCore.Weixin.EntityFrameworkCore;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TSubscriber"></typeparam>
/// <typeparam name="TContext"></typeparam>
public class SubscriberStore<TSubscriber, TContext> : ISubscriberStore<TSubscriber>
    where TSubscriber : Subscriber, new()
    where TContext : DbContext, IWeixinDbContext<TSubscriber>
{
    private bool _disposed;

    /// <summary>
    /// Gets or sets the <see cref="WeixinErrorDescriber"/> for any error that occurred with the current operation.
    /// </summary>
    public WeixinErrorDescriber ErrorDescriber { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="describer"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public SubscriberStore(TContext context, WeixinErrorDescriber describer = null)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }
        ErrorDescriber = describer ?? new WeixinErrorDescriber();
        Context = context;
    }

    /// <summary>
    /// Gets the database context for this store.
    /// </summary>
    public virtual TContext Context { get; private set; }

    private DbSet<TSubscriber> SubscribersSet { get { return Context.Set<TSubscriber>(); } }

    /// <summary>
    /// A navigation property for the <see cref="Subscriber"/> the store contains.
    /// </summary>
    public IQueryable<TSubscriber> Subscribers { get { return SubscribersSet; } }

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

    /// <summary>
    /// Dispose the stores
    /// </summary>
    public void Dispose() => _disposed = true;

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

    /// <summary>
    /// Creates a new <see cref="Subscriber"/> in a store as an asynchronous operation.
    /// </summary>
    /// <param name="subscriber">The <see cref="Subscriber"/> to create in the store.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="WeixinResult"/> of the asynchronous query.</returns>
    public async virtual Task<WeixinResult> CreateAsync(TSubscriber subscriber, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (subscriber == null)
        {
            throw new ArgumentNullException(nameof(subscriber));
        }
        Context.Add(subscriber);
        await SaveChanges(cancellationToken);
        return WeixinResult.Success;
    }

    /// <summary>
    /// Updates a <see cref="Subscriber"/> in a store as an asynchronous operation.
    /// </summary>
    /// <param name="subscriber">The <see cref="Subscriber"/> to update in the store.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="WeixinResult"/> of the asynchronous query.</returns>
    public async virtual Task<WeixinResult> UpdateAsync(TSubscriber subscriber, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (subscriber == null)
        {
            throw new ArgumentNullException(nameof(subscriber));
        }
        Context.Attach(subscriber);
        Context.Update(subscriber);
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
    /// Deletes a role from the store as an asynchronous operation.
    /// </summary>
    /// <param name="role">The role to delete from the store.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="WeixinResult"/> of the asynchronous query.</returns>
    public async virtual Task<WeixinResult> DeleteAsync(TSubscriber role, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (role == null)
        {
            throw new ArgumentNullException(nameof(role));
        }
        Context.Remove(role);
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
    /// Gets the OpenId for a <see cref="Subscriber"/> from the store as an asynchronous operation.
    /// </summary>
    /// <param name="subscriber">The <see cref="Subscriber"/> whose OpenId should be returned.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>A <see cref="Task{TResult}"/> that contains the OpenId of the <see cref="Subscriber"/>.</returns>
    public virtual Task<string> GetOpenIdAsync(TSubscriber subscriber, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (subscriber == null)
        {
            throw new ArgumentNullException(nameof(subscriber));
        }
        return Task.FromResult(subscriber.OpenId);
    }

    /// <summary>
    /// Gets the UnionId for a <see cref="Subscriber"/> from the store as an asynchronous operation.
    /// </summary>
    /// <param name="subscriber">The <see cref="Subscriber"/> whose UnionId should be returned.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>A <see cref="Task{TResult}"/> that contains the UnionId of the <see cref="Subscriber"/>.</returns>
    public virtual Task<string> GetUnionIdAsync(TSubscriber subscriber, CancellationToken cancellationToken = default(CancellationToken))
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (subscriber == null)
        {
            throw new ArgumentNullException(nameof(subscriber));
        }
        return Task.FromResult(subscriber.UnionId);
    }

    /// <summary>
    /// Gets the UserId for a <see cref="Subscriber"/> from the store as an asynchronous operation.
    /// </summary>
    /// <param name="subscriber">The <see cref="Subscriber"/> whose UserId should be returned.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>A <see cref="Task{TResult}"/> that contains the UserId of the <see cref="Subscriber"/>.</returns>
    public virtual Task<string> GetUserIdAsync(TSubscriber subscriber, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (subscriber == null)
        {
            throw new ArgumentNullException(nameof(subscriber));
        }
        return Task.FromResult(subscriber.UserId);
    }

    /// <inheritdoc/>
    public virtual Task SetUserIdAsync(TSubscriber subscriber, string userId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (subscriber == null)
        {
            throw new ArgumentNullException(nameof(subscriber));
        }
        subscriber.UserId = userId;
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public virtual Task SetMentorIdAsync(TSubscriber subscriber, string userId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (subscriber == null)
        {
            throw new ArgumentNullException(nameof(subscriber));
        }
        subscriber.MentorId = userId;
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public virtual Task SetAppIdAsync(TSubscriber subscriber, string appId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (subscriber == null)
        {
            throw new ArgumentNullException(nameof(subscriber));
        }
        subscriber.AppId = appId;
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public virtual async Task RemoveSubscriberAsync(TSubscriber subscriber, string openId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (subscriber == null)
        {
            throw new ArgumentNullException(nameof(subscriber));
        }
        var entry = await FindByOpenIdAsync(subscriber.OpenId, cancellationToken);
        if (entry != null)
        {
            SubscribersSet.Remove(entry);
        }
    }

    /// <inheritdoc/>
    public virtual Task<int> GetSubscribersCountAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        return Subscribers.CountAsync();
    }

    /// <inheritdoc/>
    public virtual async Task<IList<TSubscriber>> GetSubscribersAsync(int perPage, int pageIndex, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        return await Subscribers.Skip(pageIndex * perPage).Take(perPage).ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<TSubscriber> FindByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (userId == null)
        {
            throw new ArgumentNullException(nameof(userId));
        }
        //var userId = user.Id;
        return await Subscribers.FirstOrDefaultAsync(l => l.UserId.Equals(userId), cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<TSubscriber> FindByOpenIdAsync(string openId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (openId == null)
        {
            throw new ArgumentNullException(nameof(openId));
        }
        //var userId = user.Id;
        return await Subscribers.FirstOrDefaultAsync(l => l.OpenId.Equals(openId), cancellationToken);
    }

    /// <inheritdoc/>
    public virtual Task<TSubscriber> FindByUnionIdAsync(string unionId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public virtual Task<TSubscriber> FindByNickNameAsync(string nickname, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
