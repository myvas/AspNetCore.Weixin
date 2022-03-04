using AutoMapper;
using Myvas.AspNetCore.Weixin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin.EntityFrameworkCore.Mappers;

/// <summary>
/// Defines entity/model <see cref="Subscriber"/> and json <see cref="UserInfoJson"/> mapping.
/// </summary>
public class WeixinSubscriberMapperProfile : Profile
{
    /// <summary>
    /// </summary>
    public WeixinSubscriberMapperProfile()
    {
        CreateMap<Subscriber, UserInfoJson>(MemberList.Destination)
            .ReverseMap();
    }
}