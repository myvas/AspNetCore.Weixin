using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TWeixinSubscriber"></typeparam>
public interface IQueryableWeixinSubscriberStore<TWeixinSubscriber> : IWeixinSubscriberStore<TWeixinSubscriber>
    where TWeixinSubscriber : class
{
    /// <summary>
    /// Returns an <see cref="IQueryable{T}"/> collection of roles.
    /// </summary>
    /// <value>An <see cref="IQueryable{T}"/> collection of roles.</value>
    IQueryable<TWeixinSubscriber> WeixinSubscribers { get; }
}
