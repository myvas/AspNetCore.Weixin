using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// A shortcut of <see cref="IEntity{TKey}"/> for most used primary key type (<see cref="string"/>).
    /// </summary>
    public interface IEntity : IEntity<string>
    {

    }

    /// <summary>
    /// Defines interface for base entity type. All entities in the system must implement this interface.
    /// </summary>
    /// <typeparam name="TKey">Type of the primary key of the entity</typeparam>
    public interface IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        TKey Id { get; set; }

        /// <summary>
        /// Checks if this entity is transient (not persisted to database and it has not an <see cref="Id"/>).
        /// </summary>
        /// <returns>True, if this entity is transient</returns>
        //bool IsTransient();
    }
}
