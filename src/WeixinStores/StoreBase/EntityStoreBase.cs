using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin
{
    public abstract class EntityStoreBase<TEntity> : IEntityStore<TEntity>, IQueryableEntityStore<TEntity>
        where TEntity : class, IEntity
    {
        public abstract IQueryable<TEntity> Items { get; }

        #region IDispose
        private bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                //Items.Dispose();
                _disposed = true;
            }
        }

        /// <summary>
        /// Throws if this class has been disposed.
        /// </summary>
        protected void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }
        #endregion

        public abstract Task<WeixinResult> CreateAsync(TEntity entity, CancellationToken cancellationToken);
        public abstract Task<WeixinResult> DeleteAsync(TEntity entity, CancellationToken cancellationToken);
        public abstract Task<WeixinResult> UpdateAsync(TEntity entity, CancellationToken cancellationToken);

        public virtual Task<TEntity> FindByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var result = Items.FirstOrDefault(x => x.Id == id);
            return Task.FromResult(result);
        }

        public virtual Task<int> GetCountAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var result = Items.Count();
            return Task.FromResult(result);
        }

        public virtual Task<string> GetIdAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            var result = entity.Id;
            return Task.FromResult(result);
        }

        public virtual Task<IList<TEntity>> GetItemsAsync(int perPage, int pageIndex, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var result = Items.Skip(perPage * pageIndex).Take(perPage).ToList();
            return Task.FromResult((IList<TEntity>)result);
        }

    }
}
