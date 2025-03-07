using Myvas.AspNetCore.Weixin.Models;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// Provides an abstraction for querying subscribers in a Subscriber store.
/// </summary>
/// <typeparam name="TReceivedEntry">The type encapsulating a message.</typeparam>
public interface IQueryableReceivedEntryStore<TReceivedEntry> : IReceivedEntryStore<TReceivedEntry>
    where TReceivedEntry : ReceivedEntry
{
    /// <summary>
    /// Returns an <see cref="IQueryable{TSubscriber}"/> collection of messages.
    /// </summary>
    /// <value>An <see cref="IQueryable{TSubscriber}"/> collection of messages.</value>
    IQueryable<TReceivedEntry> ReceivedEntries { get; }
}