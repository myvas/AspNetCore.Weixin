using AutoMapper;
using Myvas.AspNetCore.Weixin.Models;

namespace Myvas.AspNetCore.Weixin.EntityFrameworkCore;

/// <summary>
/// Extension methods to map to/from json/entity.
/// </summary>
public static class UserInfoJsonMappers
{
    internal static IMapper Mapper { get; }

    static UserInfoJsonMappers()
    {
        Mapper = new MapperConfiguration(x => x.AddProfile<UserInfoJsonMapperProfile>())
            .CreateMapper();
    }

    /// <summary>
    /// Maps an entity to a json.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns></returns>
    public static UserInfoJson ToModel(this Subscriber entity)
    {
        return entity == null ? null : Mapper.Map<UserInfoJson>(entity);
    }

    /// <summary>
    /// Maps a json to an entity.
    /// </summary>
    /// <param name="model">The model</param>
    /// <returns></returns>
    public static Subscriber ToEntity(this UserInfoJson model)
    {
        return model == null ? null : Mapper.Map<Subscriber>(model);
    }

    /// <summary>
    /// Updates an entity from a json.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <param name="entity">The entity.</param>
    public static void UpdateEntity(this UserInfoJson model, Subscriber entity)
    {
        Mapper.Map(model, entity);
    } 
}
