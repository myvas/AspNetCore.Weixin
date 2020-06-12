using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin.EntityFrameworkCore
{
    public class WeixinSendMessageStore<TContext> : WeixinSendMessageStore<WeixinSendMessage, TContext>, IWeixinSendMessageStore
        where TContext : DbContext
    {
        public WeixinSendMessageStore(TContext context, WeixinErrorDescriber describer = null) : base(context, describer)
        {
        }
    }

    public class WeixinSendMessageStore<TWeixinSendMessage, TContext> : WeixinSendMessageStoreBase<TWeixinSendMessage>
        where TWeixinSendMessage : WeixinSendMessage
        where TContext : DbContext
    {
        public WeixinSendMessageStore(TContext context, WeixinErrorDescriber describer = null)
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

        private DbSet<TWeixinSendMessage> MessagesSet { get { return Context.Set<TWeixinSendMessage>(); } }
        public override IQueryable<TWeixinSendMessage> Items => Context.Set<TWeixinSendMessage>();

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

        public override Task<WeixinResult> CreateAsync(TWeixinSendMessage item, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<WeixinResult> UpdateAsync(TWeixinSendMessage item, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<WeixinResult> DeleteAsync(TWeixinSendMessage item, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
