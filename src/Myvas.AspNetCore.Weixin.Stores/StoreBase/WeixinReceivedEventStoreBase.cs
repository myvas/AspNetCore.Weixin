using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin
{
    public abstract class WeixinReceivedEventStoreBase<TWeixinReceivedEvent> : EntityStoreBase<TWeixinReceivedEvent>, IWeixinReceivedEventStore<TWeixinReceivedEvent>
        where TWeixinReceivedEvent : WeixinReceivedEvent
    {
        public WeixinReceivedEventStoreBase(WeixinErrorDescriber describer)
        {
            ErrorDescriber = describer ?? throw new ArgumentNullException(nameof(describer));
        }

        /// <summary>
        /// Gets or sets the <see cref="WeixinErrorDescriber"/> for any error that occurred with the current operation.
        /// </summary>
        public WeixinErrorDescriber ErrorDescriber { get; set; }

        public virtual Task<int> GetCountByOpenIdAsync(string openId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }


        public virtual Task<IList<TWeixinReceivedEvent>> GetItemsByOpenIdAsync(string openId, int perPage, int pageIndex, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
