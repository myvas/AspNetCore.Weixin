﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public abstract class WeixinSendMessageStoreBase<TWeixinSendMessageEntity> : EntityStoreBase<TWeixinSendMessageEntity>, IWeixinSendMessageStore<TWeixinSendMessageEntity>
    where TWeixinSendMessageEntity : WeixinSendMessageEntity
{
    public WeixinSendMessageStoreBase(WeixinErrorDescriber describer)
    {
        ErrorDescriber = describer ?? throw new ArgumentNullException(nameof(describer));
    }

    /// <summary>
    /// Gets or sets the <see cref="WeixinErrorDescriber"/> for any error that occurred with the current operation.
    /// </summary>
    public WeixinErrorDescriber ErrorDescriber { get; set; }

}
