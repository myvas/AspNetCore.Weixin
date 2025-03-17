using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Myvas.AspNetCore.Weixin.Models;
using System.Linq;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// Provides an abstraction for querying subscribers in a Subscriber store.
/// </summary>
/// <typeparam name="TSubscriber">The type encapsulating a subscriber.</typeparam>
public interface IQueryableSubscriberStore<TSubscriber> : ISubscriberStore<TSubscriber>
    where TSubscriber : Subscriber
{
    /// <summary>
    /// Returns an <see cref="IQueryable{TSubscriber}"/> collection of subscribers.
    /// </summary>
    /// <value>An <see cref="IQueryable{TSubscriber}"/> collection of subscribers.</value>
    IQueryable<TSubscriber> Subscribers { get; }
}
