using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin.EntityFrameworkCore.Mappers;

/// <summary>
/// Defines entity/model mapping.
/// </summary>
public class WeixinSubscriberMapperProfile : Profile
{
    /// <summary>
    /// </summary>
    public WeixinSubscriberMapperProfile()
    {
        CreateMap<Entities.WeixinSubscriber, Models.WeixinSubscriber>(MemberList.Destination)
            .ReverseMap();
    }
}