using System.Linq;

namespace Myvas.AspNetCore.Weixin
{
    public interface IQueryableEntityStore<TEntity>
        where TEntity: class
    {
        /// <summary>
        /// Return an <see cref="IQueryable{TEntity}"/> collection of entities.
        /// </summary>
        /// <value>An <see cref="IQueryable{TEntity}"/> collection of entities.</value>
        IQueryable<TEntity> Items { get; }
    }
}
