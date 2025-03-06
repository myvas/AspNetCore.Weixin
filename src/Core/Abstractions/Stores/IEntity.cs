using System;

namespace Myvas.AspNetCore.Weixin
{
    public interface IEntity : IEntity<string>
    {
    }

    public interface IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Gets or sets the primary key for this entity.
        /// </summary>
        TKey Id { get; set; }
    }
}
