using AutoMapper;
using Myvas.AspNetCore.Weixin.Models;

namespace Myvas.AspNetCore.Weixin.EntityFrameworkCore.Mappers;

/// <summary>
/// Extension methods to map to/from json/model.
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
    public static UserInfoJson ToModel(this WeixinSubscriber entity)
    {
        return entity == null ? null : Mapper.Map<UserInfoJson>(entity);
    }

    /// <summary>
    /// Maps a model to an entity.
    /// </summary>
    /// <param name="model">The model</param>
    /// <returns></returns>
    public static WeixinSubscriber ToEntity(this UserInfoJson model)
    {
        return model == null ? null : Mapper.Map<WeixinSubscriber>(model);
    }

    /// <summary>
    /// Updates an entity from a model.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <param name="entity">The entity.</param>
    public static void UpdateEntity(this UserInfoJson model, WeixinSubscriber entity)
    {
        Mapper.Map(model, entity);
    } 
}
