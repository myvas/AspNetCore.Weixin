using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace Myvas.AspNetCore.Weixin
{
    public interface IEntityStore<TEntity> : IDisposable
        where TEntity : class, IEntity
    {
        /// <summary>
        /// Creates a new entity in a store as an asynchronous operation.
        /// </summary>
        /// <param name="entity">The entity to create in the store.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="WeixinResult"/> of the asynchronous query.</returns>
        Task<WeixinResult> CreateAsync(TEntity entity, CancellationToken cancellationToken);

        /// <summary>
        /// Updates an entity in a store as an asynchronous operation.
        /// </summary>
        /// <param name="entity">The entity to update in the store.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="WeixinResult"/> of the asynchronous query.</returns>
        Task<WeixinResult> UpdateAsync(TEntity entity, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes an entity from the store as an asynchronous operation.
        /// </summary>
        /// <param name="entity">The entity to delete from the store.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="WeixinResult"/> of the asynchronous query.</returns>
        Task<WeixinResult> DeleteAsync(TEntity entity, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the ID for an entity from the store as an asynchronous operation.
        /// </summary>
        /// <param name="entity">The entity whose ID should be returned.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that contains the ID of the role.</returns>
        Task<string> GetIdAsync(TEntity entity, CancellationToken cancellationToken);

        /// <summary>
        /// Finds the entity who has the specified ID as an asynchronous operation.
        /// </summary>
        /// <param name="id">The entity ID to look for.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that result of the look up.</returns>
        Task<TEntity> FindByIdAsync(string id, CancellationToken cancellationToken);

        Task<int> GetCountAsync(CancellationToken cancellationToken = default);

        Task<IList<TEntity>> GetItemsAsync(int perPage, int pageIndex, CancellationToken cancellationToken = default);
    }
}
