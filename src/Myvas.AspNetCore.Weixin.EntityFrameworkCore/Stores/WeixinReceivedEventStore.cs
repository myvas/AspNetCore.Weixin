using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin.EntityFrameworkCore
{
    public class WeixinReceivedEventStore<TWeixinReceivedEvent> : WeixinReceivedEventStore<TWeixinReceivedEvent, DbContext>
        where TWeixinReceivedEvent : WeixinReceivedEvent
    {
        public WeixinReceivedEventStore(DbContext context, WeixinErrorDescriber describer = null) : base(context, describer)
        {
        }
    }

    public class WeixinReceivedEventStore<TWeixinReceivedEvent, TContext> : WeixinReceivedEventStoreBase<TWeixinReceivedEvent>
        where TWeixinReceivedEvent : WeixinReceivedEvent
        where TContext : DbContext
    {
        public WeixinReceivedEventStore(TContext context, WeixinErrorDescriber describer = null)
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

        private DbSet<TWeixinReceivedEvent> MessagesSet { get { return Context.Set<TWeixinReceivedEvent>(); } }

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

        public override Task<WeixinResult> CreateAsync(TWeixinReceivedEvent item, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<WeixinResult> UpdateAsync(TWeixinReceivedEvent item, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<WeixinResult> DeleteAsync(TWeixinReceivedEvent item, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
