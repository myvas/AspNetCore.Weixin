using AutoMapper;

namespace Myvas.AspNetCore.Weixin.EntityFrameworkCore.Mappers;

/// <summary>
/// Extension methods to map to/from entity/model.
/// </summary>
public static class WeixinSubscriberMappers
{
    static WeixinSubscriberMappers()
    {
        Mapper = new MapperConfiguration(x => x.AddProfile<WeixinSubscriberMapperProfile>())
            .CreateMapper();
    }

    internal static IMapper Mapper { get; }

    /// <summary>
    /// Maps an entity to a model.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns></returns>
    public static Models.WeixinSubscriber ToModel(this Entities.WeixinSubscriber entity)
    {
        return entity == null ? null : Mapper.Map<Models.WeixinSubscriber>(entity);
    }

    /// <summary>
    /// Maps a model to an entity.
    /// </summary>
    /// <param name="model">The model</param>
    /// <returns></returns>
    public static Entities.WeixinSubscriber ToEntity(this Models.WeixinSubscriber model)
    {
        return model == null ? null : Mapper.Map<Entities.WeixinSubscriber>(model);
    }

    /// <summary>
    /// Updates an entity from a model.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <param name="entity">The entity.</param>
    public static void UpdateEntity(this Models.WeixinSubscriber model, Entities.WeixinSubscriber entity)
    {
        Mapper.Map(model, entity);
    } 
}
