﻿using Microsoft.EntityFrameworkCore;
using Myvas.AspNetCore.Weixin.Models;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin.EntityFrameworkCore;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TWeixinSubscriber"></typeparam>
public class SubscriberOnlyStore<TWeixinSubscriber>
    : SubscriberOnlyStore<TWeixinSubscriber, DbContext>
    where TWeixinSubscriber : WeixinSubscriber, new()
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="describer"></param>
    public SubscriberOnlyStore(DbContext context, WeixinErrorDescriber describer = null)
        : base(context, describer) { }
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="TWeixinSubscriber"></typeparam>
/// <typeparam name="TContext"></typeparam>
public class SubscriberOnlyStore<TWeixinSubscriber, TContext> : SubscriberStoreBase<TWeixinSubscriber>
    where TWeixinSubscriber : WeixinSubscriber, new()
    where TContext : DbContext
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="describer"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public SubscriberOnlyStore(TContext context, WeixinErrorDescriber describer = null)
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

    /// <summary>
    /// DbSet of users.
    /// </summary>
    protected DbSet<TWeixinSubscriber> SubscribersSet { get { return Context.Set<TWeixinSubscriber>(); } }


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
    /// Creates the specified <paramref name="subscriber"/> in the user store.
    /// </summary>
    /// <param name="subscriber">The user to create.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="WeixinResult"/> of the creation operation.</returns>
    public async override Task<WeixinResult> CreateAsync(TWeixinSubscriber subscriber, CancellationToken cancellationToken = default(CancellationToken))
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
    /// Updates the specified <paramref name="subscriber"/> in the user store.
    /// </summary>
    /// <param name="subscriber">The user to update.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="WeixinResult"/> of the update operation.</returns>
    public async override Task<WeixinResult> UpdateAsync(TWeixinSubscriber subscriber, CancellationToken cancellationToken = default(CancellationToken))
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (subscriber == null)
        {
            throw new ArgumentNullException(nameof(subscriber));
        }

        Context.Attach(subscriber);
        subscriber.ConcurrencyStamp = Guid.NewGuid().ToString();
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
    /// Deletes the specified <paramref name="subscriber"/> from the user store.
    /// </summary>
    /// <param name="subscriber">The user to delete.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="WeixinResult"/> of the update operation.</returns>
    public async override Task<WeixinResult> DeleteAsync(TWeixinSubscriber subscriber, CancellationToken cancellationToken = default(CancellationToken))
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (subscriber == null)
        {
            throw new ArgumentNullException(nameof(subscriber));
        }

        Context.Remove(subscriber);
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

    /// <inheritdoc/>
    public override Task<TWeixinSubscriber> FindByIdAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        //var id = ConvertIdFromString(subscriberId);
        return SubscribersSet.FindAsync(new object[] { id }, cancellationToken).AsTask();
    }

    /// <inheritdoc/>
    public override Task<TWeixinSubscriber> FindByNicknameAsync(string nickname, CancellationToken cancellationToken = default(CancellationToken))
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        //var id = ConvertIdFromString(subscriberId);
        return Subscribers.FirstOrDefaultAsync(x => x.Nickname == nickname, cancellationToken);
    }

    /// <inheritdoc/>
    public override Task AddSubscriberAsync(TWeixinSubscriber subscriber, string userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override Task RemoveSubscriberAsync(TWeixinSubscriber subscriber, string userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override Task<int> GetSubscribersCountAsync()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override Task<IList<TWeixinSubscriber>> GetSubscribersAsync(int perPage, int pageIndex, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override Task<TWeixinSubscriber> FindByUserIdAsync(string userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override Task<TWeixinSubscriber> FindByOpenIdAsync(string openId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override Task<TWeixinSubscriber> FindByUnionIdAsync(string unionId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override IQueryable<TWeixinSubscriber> Subscribers
    {
        get { return SubscribersSet; }
    }
}
