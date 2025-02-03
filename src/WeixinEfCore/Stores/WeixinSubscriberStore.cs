using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin.EntityFrameworkCore
{
    public class WeixinSubscriberStore<TContext> : WeixinSubscriberStore<WeixinSubscriber, string, TContext>, IWeixinSubscriberStore
        where TContext : DbContext
    {
        public WeixinSubscriberStore(TContext context, WeixinErrorDescriber describer = null) : base(context, describer)
        {
        }
    }

    public class WeixinSubscriberStore<TWeixinSubscriber, TKey, TContext> : WeixinSubscriberStoreBase<TWeixinSubscriber, TKey>
        where TWeixinSubscriber : WeixinSubscriber<TKey>
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

        public override IQueryable<TWeixinSubscriber> Items => Context.Set<TWeixinSubscriber>();

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
        public async override Task<WeixinResult> CreateAsync(TWeixinSubscriber subscriber, CancellationToken cancellationToken = default)
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
        /// Updates a role in a store as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role to update in the store.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
        public async override Task<WeixinResult> UpdateAsync(TWeixinSubscriber subscriber, CancellationToken cancellationToken = default)
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
        /// Deletes an item from the store as an asynchronous operation.
        /// </summary>
        /// <param name="item">The item to delete from the store.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="WeixinResult"/> of the asynchronous query.</returns>
        public async override Task<WeixinResult> DeleteAsync(TWeixinSubscriber item, CancellationToken cancellationToken = default)
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
        public override Task<WeixinResult> SetUserIdAsync(TWeixinSubscriber item, TKey userId, CancellationToken cancellationToken = default)
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

        public override Task<WeixinResult> AddAssociationAsync(TWeixinSubscriber item, TKey userId, CancellationToken cancellationToken = default)
            => SetUserIdAsync(item, userId, cancellationToken);

        public override Task<WeixinResult> RemoveAssociationAsync(TWeixinSubscriber item, CancellationToken cancellationToken = default)
            => SetUserIdAsync(item, default, cancellationToken);
    }
}
