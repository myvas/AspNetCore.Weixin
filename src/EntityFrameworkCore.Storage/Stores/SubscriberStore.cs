using Microsoft.EntityFrameworkCore;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.Entities;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin.EntityFrameworkCore
{
    public class SubscriberStore<TWeixinSubscriber, TContext> : SubscriberStoreBase<TWeixinSubscriber>
        where TWeixinSubscriber : WeixinSubscriber, new()
        where TContext : DbContext
    {
        public SubscriberStore(TContext context, WeixinErrorDescriber describer = null)
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

        public override IQueryable<TWeixinSubscriber> Subscribers => throw new NotImplementedException();

        /// <summary>Saves the current store.</summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        protected Task SaveChanges(CancellationToken cancellationToken)
        {
            return AutoSaveChanges ? Context.SaveChangesAsync(cancellationToken) : Task.CompletedTask;
        }

        /// <summary>
        /// Creates a new role in a store as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role to create in the store.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
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
        /// Updates a role in a store as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role to update in the store.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
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
        /// Deletes a role from the store as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role to delete from the store.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
        public async override Task<WeixinResult> DeleteAsync(TWeixinSubscriber role, CancellationToken cancellationToken = default(CancellationToken))
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
        /// Gets the ID for a role from the store as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role whose ID should be returned.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that contains the ID of the role.</returns>
        public virtual Task<string> GetUserIdAsync(TWeixinSubscriber subscriber, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (subscriber == null)
            {
                throw new ArgumentNullException(nameof(subscriber));
            }
            return Task.FromResult(subscriber.UserId);
        }


        /// <summary>
        /// Sets the name of a role in the store as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role whose name should be set.</param>
        /// <param name="roleName">The name of the role.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        public override Task SetUserIdAsync(TWeixinSubscriber subscriber, string userId, CancellationToken cancellationToken = default(CancellationToken))
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
        public override Task<TWeixinSubscriber> FindByIdAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            //var roleId = ConvertIdFromString(id);
            return SubscribersSet.FirstOrDefaultAsync(u => u.Id.Equals(id), cancellationToken);
        }

        public override Task AddSubscriberAsync(TWeixinSubscriber subscriber, string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (subscriber == null)
            {
                throw new ArgumentNullException(nameof(subscriber));
            }
            SubscribersSet.Add(subscriber);
            return Task.FromResult(false);
        }

        public override async Task RemoveSubscriberAsync(TWeixinSubscriber subscriber, string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (subscriber == null)
            {
                throw new ArgumentNullException(nameof(subscriber));
            }
            var entry = await FindByIdAsync(subscriber.Id, cancellationToken);
            if (entry != null)
            {
                SubscribersSet.Remove(entry);
            }
        }

        public override Task<int> GetSubscribersCountAsync()
        {
            throw new NotImplementedException();
        }

        public override async Task<IList<TWeixinSubscriber>> GetSubscribersAsync(int perPage, int pageIndex, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return await Subscribers.Skip(pageIndex * perPage).Take(perPage).ToListAsync(cancellationToken);
        }

        public override async Task<TWeixinSubscriber> FindByUserIdAsync(string userId, CancellationToken cancellationToken)
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

        public override Task<TWeixinSubscriber> FindByOpenIdAsync(string openId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<TWeixinSubscriber> FindByUnionIdAsync(string unionId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<TWeixinSubscriber> FindByNicknameAsync(string nickname, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
