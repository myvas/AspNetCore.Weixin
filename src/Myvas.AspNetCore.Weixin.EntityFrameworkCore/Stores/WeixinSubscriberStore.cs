using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin.EntityFrameworkCore
{
    public class WeixinSubscriberStore<TWeixinSubscriber> : WeixinSubscriberStore<TWeixinSubscriber, string>
        where TWeixinSubscriber : WeixinSubscriber
    {
        /// <summary>
        /// Constructs a new instance of <see cref="WeixinSubscriberStore{TWeixinSubscriber}"/>.
        /// </summary>
        /// <param name="context">The <see cref="DbContext"/>.</param>
        /// <param name="describer">The <see cref="IdentityErrorDescriber"/>.</param>
        public WeixinSubscriberStore(DbContext context, WeixinErrorDescriber describer = null) : base(context, describer)
        {
        }
    }

    public class WeixinSubscriberStore<TWeixinSubscriber, TKey> : WeixinSubscriberStore<TWeixinSubscriber, TKey, DbContext>
        where TWeixinSubscriber : WeixinSubscriber<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Constructs a new instance of <see cref="WeixinSubscriberStore{TWeixinSubscriber}"/>.
        /// </summary>
        /// <param name="context">The <see cref="DbContext"/>.</param>
        /// <param name="describer">The <see cref="IdentityErrorDescriber"/>.</param>
        public WeixinSubscriberStore(DbContext context, WeixinErrorDescriber describer = null) : base(context, describer)
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

        private DbSet<TWeixinSubscriber> SubscribersSet { get { return Context.Set<TWeixinSubscriber>(); } }

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
        /// Gets the ID for an item from the store as an asynchronous operation.
        /// </summary>
        /// <param name="role">The item whose ID should be returned.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that contains the ID of the item.</returns>
        public virtual Task<TKey> GetUserIdAsync(TWeixinSubscriber item, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            return Task.FromResult(item.UserId);
        }


        /// <summary>
        /// Sets the name of a role in the store as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role whose name should be set.</param>
        /// <param name="roleName">The name of the role.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        public override Task SetUserIdAsync(TWeixinSubscriber subscriber, TKey userId, CancellationToken cancellationToken = default)
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


        /// <summary>
        /// Finds the role who has the specified ID as an asynchronous operation.
        /// </summary>
        /// <param name="id">The role ID to look for.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that result of the look up.</returns>
        public override Task<TWeixinSubscriber> FindByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            //var roleId = ConvertIdFromString(id);
            return SubscribersSet.FirstOrDefaultAsync(u => u.Id.Equals(id), cancellationToken);
        }

        public override Task AddAssociationAsync(TWeixinSubscriber item, TKey userId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            SubscribersSet.Add(item);
            return Task.FromResult(false);
        }

        public override async Task RemoveAssociationAsync(TWeixinSubscriber item, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            var entry = await FindByIdAsync(item.Id, cancellationToken);
            if (entry != null)
            {
                SubscribersSet.Remove(entry);
            }
        }

        public override async Task<TWeixinSubscriber> FindByUserIdAsync(TKey userId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }
            //var userId = user.Id;
            return await Items.FirstOrDefaultAsync(l => l.UserId.Equals(userId), cancellationToken);
        }

        public override Task<TWeixinSubscriber> FindByOpenIdAsync(string openId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
