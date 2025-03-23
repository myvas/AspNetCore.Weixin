using Myvas.AspNetCore.Weixin;

public class EntitiesViewModel<TEntity>
    where TEntity: class, IEntity
{
    public IList<TEntity> Items { get; set; } = [];
    public int PageSize { get; set; }
    public int TotalRecords { get; set; }
    public int TotalPages { get; set; }
    public int PageIndex { get; set; }
}