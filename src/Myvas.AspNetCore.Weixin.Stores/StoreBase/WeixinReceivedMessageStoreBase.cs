﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin
{
    public abstract class WeixinReceivedMessageStoreBase<TWeixinReceivedMessage> : EntityStoreBase<TWeixinReceivedMessage>, IWeixinReceivedMessageStore<TWeixinReceivedMessage>
        where TWeixinReceivedMessage : WeixinReceivedMessage
    {
        public WeixinReceivedMessageStoreBase(WeixinErrorDescriber describer)
        {
            ErrorDescriber = describer ?? throw new ArgumentNullException(nameof(describer));
        }

        /// <summary>
        /// Gets or sets the <see cref="WeixinErrorDescriber"/> for any error that occurred with the current operation.
        /// </summary>
        public WeixinErrorDescriber ErrorDescriber { get; set; }

        public virtual Task<int> GetCountBySourceOpenIdAsync(string openId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IList<TWeixinReceivedMessage>> GetItemsBySourceOpenIdAsync(string openId, int perPage, int pageIndex, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IList<TWeixinReceivedMessage>> GetResponseMessagesAsync(string id, int perPage, int pageIndex, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
