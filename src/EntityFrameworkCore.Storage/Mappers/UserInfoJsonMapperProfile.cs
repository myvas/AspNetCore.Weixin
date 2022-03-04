using AutoMapper;
using Myvas.AspNetCore.Weixin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin.EntityFrameworkCore;

/// <summary>
/// Defines <see cref="UserInfoJson"/> and <see cref="Subscriber"/> mapping.
/// </summary>
public class UserInfoJsonMapperProfile : Profile
{
    /// <summary>
    /// </summary>
    public UserInfoJsonMapperProfile()
    {
        CreateMap<Subscriber, UserInfoJson>(MemberList.Destination)
            .ReverseMap();
    }
}