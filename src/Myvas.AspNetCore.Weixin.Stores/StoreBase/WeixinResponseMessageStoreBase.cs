﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin
{
    public abstract class WeixinResponseMessageStoreBase<TWeixinResponseMessage> : EntityStoreBase<TWeixinResponseMessage>, IWeixinResponseMessageStore<TWeixinResponseMessage>
        where TWeixinResponseMessage : WeixinResponseMessage
    {
        public WeixinResponseMessageStoreBase(WeixinErrorDescriber describer)
        {
            ErrorDescriber = describer ?? throw new ArgumentNullException(nameof(describer));
        }

        /// <summary>
        /// Gets or sets the <see cref="WeixinErrorDescriber"/> for any error that occurred with the current operation.
        /// </summary>
        public WeixinErrorDescriber ErrorDescriber { get; set; }

        public virtual Task<int> GetCountByDestOpenIdAsync(string openId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IList<TWeixinResponseMessage>> GetItemsByDestOpenIdAsync(string openId, int perPage, int pageIndex, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IList<TWeixinResponseMessage>> GetRequestMessagesAsync(string id, int perPage, int pageIndex, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
