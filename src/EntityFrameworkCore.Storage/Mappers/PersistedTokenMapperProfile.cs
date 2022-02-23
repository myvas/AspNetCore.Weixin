using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin.AccessTokenServer.EntityFrameworkCore.Mappers;

/// <summary>
/// Defines entity/model mapping for persisted tokens.
/// </summary>
public class PersistedTokenMapperProfile : Profile
{
    /// <summary>
    /// <see cref="PersistedTokenMapperProfile">
    /// </see>
    /// </summary>
    public PersistedTokenMapperProfile()
    {
        CreateMap<Entities.PersistedToken, Models.PersistedToken>(MemberList.Destination)
            .ReverseMap();
    }
}