using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace Myvas.AspNetCore.Weixin
{
    public interface IWeixinSendMessageStore : IWeixinSendMessageStore<WeixinSendMessage> { }

    /// <summary>
    /// Provides an abstraction for storing information of messages to Weixin subscribers.
    /// </summary>
    /// <typeparam name="IWeixinSendMessage">The type that represents a message to a Weixin subscriber.</typeparam>
    public interface IWeixinSendMessageStore<TWeixinSendMessage> : IEntityStore<TWeixinSendMessage>, IQueryableEntityStore<TWeixinSendMessage>
        where TWeixinSendMessage : class, IEntity
    {
    }
}
