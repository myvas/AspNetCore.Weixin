using Myvas.AspNetCore.Weixin.Entities;

namespace Myvas.AspNetCore.Weixin.EntityFrameworkCore.Stores;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TWeixinSubscriber"></typeparam>
public abstract class SubscriberStoreBase<TWeixinSubscriber> :
    IWeixinSubscriberStore<TWeixinSubscriber>
    where TWeixinSubscriber : WeixinSubscriber, new()
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="describer"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public SubscriberStoreBase(WeixinErrorDescriber describer)
    {
        if (describer == null)
        {
            throw new ArgumentNullException(nameof(describer));
        }
        ErrorDescriber = describer;
    }

    private bool _disposed;

    /// <summary>
    /// Gets or sets the <see cref="WeixinErrorDescriber"/> for any error that occurred with the current operation.
    /// </summary>
    public WeixinErrorDescriber ErrorDescriber { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="role"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public abstract Task<WeixinResult> CreateAsync(TWeixinSubscriber role, CancellationToken cancellationToken = default(CancellationToken));


    /// <summary>
    /// Updates a role in a store as an asynchronous operation.
    /// </summary>
    /// <param name="role">The role to update in the store.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="WeixinResult"/> of the asynchronous query.</returns>
    public abstract Task<WeixinResult> UpdateAsync(TWeixinSubscriber role, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Deletes a role from the store as an asynchronous operation.
    /// </summary>
    /// <param name="role">The role to delete from the store.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="WeixinResult"/> of the asynchronous query.</returns>
    public abstract Task<WeixinResult> DeleteAsync(TWeixinSubscriber role, CancellationToken cancellationToken = default(CancellationToken));


    /// <summary>
    /// Gets the ID for a role from the store as an asynchronous operation.
    /// </summary>
    /// <param name="subscriber">The role whose ID should be returned.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>A <see cref="Task{TResult}"/> that contains the ID of the role.</returns>
    public virtual Task<string> GetIdAsync(TWeixinSubscriber subscriber, CancellationToken cancellationToken = default(CancellationToken))
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (subscriber == null)
        {
            throw new ArgumentNullException(nameof(subscriber));
        }
        return Task.FromResult(subscriber.Id);
    }

    /// <summary>
    /// Gets the ID for a role from the store as an asynchronous operation.
    /// </summary>
    /// <param name="subscriber">The subscriber whose ID should be returned.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>A <see cref="Task{TResult}"/> that contains the ID of the role.</returns>
    public virtual Task<string> GetOpenIdAsync(TWeixinSubscriber subscriber, CancellationToken cancellationToken = default(CancellationToken))
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
    /// Gets the ID for a role from the store as an asynchronous operation.
    /// </summary>
    /// <param name="subscriber">The role whose ID should be returned.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>A <see cref="Task{TResult}"/> that contains the ID of the role.</returns>
    public virtual Task<string> GetUnionIdAsync(TWeixinSubscriber subscriber, CancellationToken cancellationToken = default(CancellationToken))
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
    /// Gets the ID for a role from the store as an asynchronous operation.
    /// </summary>
    /// <param name="subscriber">The role whose ID should be returned.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>A <see cref="Task{TResult}"/> that contains the ID of the role.</returns>
    public virtual Task<string> GetNicknameAsync(TWeixinSubscriber subscriber, CancellationToken cancellationToken = default(CancellationToken))
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (subscriber == null)
        {
            throw new ArgumentNullException(nameof(subscriber));
        }
        return Task.FromResult(subscriber.Nickname);
    }

    /// <inheritdoc/>
    public virtual Task SetUserIdAsync(TWeixinSubscriber subscriber, string userId, CancellationToken cancellationToken = default(CancellationToken))
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
    public virtual Task SetMentorIdAsync(TWeixinSubscriber subscriber, string userId, CancellationToken cancellationToken = default(CancellationToken))
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
    /// Dispose the stores
    /// </summary>
    public void Dispose() => _disposed = true;

    /// <inheritdoc/>
    public abstract Task AddSubscriberAsync(TWeixinSubscriber subscriber, string userId, CancellationToken cancellationToken);

    /// <inheritdoc/>
    public abstract Task RemoveSubscriberAsync(TWeixinSubscriber subscriber, string userId, CancellationToken cancellationToken);

    /// <inheritdoc/>
    public abstract Task<int> GetSubscribersCountAsync();

    /// <inheritdoc/>
    public abstract Task<IList<TWeixinSubscriber>> GetSubscribersAsync(int perPage, int pageIndex, CancellationToken cancellationToken);

    /// <inheritdoc/>
    public abstract Task<TWeixinSubscriber> FindByIdAsync(string id, CancellationToken cancellationToken);

    /// <inheritdoc/>
    public abstract Task<TWeixinSubscriber> FindByUserIdAsync(string userId, CancellationToken cancellationToken);

    /// <inheritdoc/>
    public abstract Task<TWeixinSubscriber> FindByOpenIdAsync(string openId, CancellationToken cancellationToken);

    /// <inheritdoc/>
    public abstract Task<TWeixinSubscriber> FindByUnionIdAsync(string unionId, CancellationToken cancellationToken);

    /// <inheritdoc/>
    public abstract Task<TWeixinSubscriber> FindByNicknameAsync(string nickname, CancellationToken cancellationToken);


    /// <summary>
    /// A navigation property for the roles the store contains.
    /// </summary>
    public abstract IQueryable<TWeixinSubscriber> Subscribers
    {
        get;
    }
}
