using AutoMapper;

namespace Myvas.AspNetCore.Weixin.EntityFrameworkCore.Mappers;

/// <summary>
/// Extension methods to map to/from entity/model for persisted tokens.
/// </summary>
public static class PersistedTokenMappers
{
    static PersistedTokenMappers()
    {
        Mapper = new MapperConfiguration(x => x.AddProfile<PersistedTokenMapperProfile>())
            .CreateMapper();
    }

    internal static IMapper Mapper { get; }

    /// <summary>
    /// Maps an entity to a model.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns></returns>
    public static Models.PersistedToken ToModel(this Entities.PersistedToken entity)
    {
        return entity == null ? null : Mapper.Map<Models.PersistedToken>(entity);
    }

    /// <summary>
    /// Maps a model to an entity.
    /// </summary>
    /// <param name="model">The model</param>
    /// <returns></returns>
    public static Entities.PersistedToken ToEntity(this Models.PersistedToken model)
    {
        return model == null ? null : Mapper.Map<Entities.PersistedToken>(model);
    }

    /// <summary>
    /// Updates an entity from a model.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <param name="entity">The entity.</param>
    public static void UpdateEntity(this Models.PersistedToken model, Entities.PersistedToken entity)
    {
        Mapper.Map(model, entity);
    }
}
