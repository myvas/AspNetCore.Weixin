﻿using AutoMapper;
using Myvas.AspNetCore.Weixin.Models;

namespace Myvas.AspNetCore.Weixin.EfCore;

/// <summary>
/// Defines <see cref="ReceivedXml"/> and <see cref="ReceivedEntry"/> mapping.
/// </summary>
public class ReceivedXmlMapperProfile : Profile
{
    /// <summary>
    /// 
    /// </summary>
    public ReceivedXmlMapperProfile()
    {
        CreateMap<ReceivedXml, ReceivedEntry>(MemberList.Destination)
            .ReverseMap();
        CreateMap<ReceivedXml, WeixinReceivedMessageEntity>(MemberList.Destination);
    }
}

/// <summary>
/// Defines <see cref="MessageReceivedXml"/> and <see cref="MessageReceivedEntry"/> mapping.
/// </summary>
public class MessageReceivedXmlMapperProfile : Profile
{
    /// <summary>
    /// 
    /// </summary>
    public MessageReceivedXmlMapperProfile()
    {
        CreateMap<MessageReceivedXml, MessageReceivedEntry>(MemberList.Destination)
            .ReverseMap();
        CreateMap<MessageReceivedXml, WeixinReceivedMessageEntity>(MemberList.Destination);
    }
}


/// <summary>
/// Defines <see cref="EventReceivedXml"/> and <see cref="EventReceivedEntry"/> mapping.
/// </summary>
public class EventReceivedXmlMapperProfile : Profile
{
    /// <summary>
    /// 
    /// </summary>
    public EventReceivedXmlMapperProfile()
    {
        CreateMap<EventReceivedXml, EventReceivedEntry>(MemberList.Destination)
            .ReverseMap();
        CreateMap<EventReceivedXml, WeixinReceivedEventEntity>(MemberList.Destination);
    }
}

/// <summary>
/// Defines <see cref="TextMessageReceivedXml"/> and <see cref="TextMessageReceivedEntry"/> mapping.
/// </summary>
public class TextMessageReceivedXmlMapperProfile : Profile
{
    /// <summary>
    /// 
    /// </summary>
    public TextMessageReceivedXmlMapperProfile()
    {
        CreateMap<TextMessageReceivedXml, TextMessageReceivedEntry>(MemberList.Destination)
            .ReverseMap();
        CreateMap<TextMessageReceivedXml, WeixinReceivedMessageEntity>(MemberList.Destination);
    }
}


/// <summary>
/// Defines <see cref="ImageMessageReceivedXml"/> and <see cref="ImageMessageReceivedEntry"/> mapping.
/// </summary>
public class ImageMessageReceivedXmlMapperProfile : Profile
{
    /// <summary>
    /// 
    /// </summary>
    public ImageMessageReceivedXmlMapperProfile()
    {
        CreateMap<ImageMessageReceivedXml, ImageMessageReceivedEntry>(MemberList.Destination)
            .ReverseMap();
        CreateMap<ImageMessageReceivedXml, WeixinReceivedMessageEntity>(MemberList.Destination);
    }
}
/// <summary>
/// Defines <see cref="LinkMessageReceivedXml"/> and <see cref="LinkMessageReceivedEntry"/> mapping.
/// </summary>
public class LinkMessageReceivedXmlMapperProfile : Profile
{
    /// <summary>
    /// 
    /// </summary>
    public LinkMessageReceivedXmlMapperProfile()
    {
        CreateMap<LinkMessageReceivedXml, LinkMessageReceivedEntry>(MemberList.Destination)
            .ReverseMap();
        CreateMap<LinkMessageReceivedXml, WeixinReceivedMessageEntity>(MemberList.Destination);
    }
}
/// <summary>
/// Defines <see cref="LocationMessageReceivedXml"/> and <see cref="LocationMessageReceivedEntry"/> mapping.
/// </summary>
public class LocationMessageReceivedXmlMapperProfile : Profile
{
    /// <summary>
    /// 
    /// </summary>
    public LocationMessageReceivedXmlMapperProfile()
    {
        CreateMap<LocationMessageReceivedXml, LocationMessageReceivedEntry>(MemberList.Destination)
            .ReverseMap();
        CreateMap<LocationMessageReceivedXml, WeixinReceivedMessageEntity>(MemberList.Destination);
    }
}
/// <summary>
/// Defines <see cref="ShortVideoMessageReceivedXml"/> and <see cref="ShortVideoMessageReceivedEntry"/> mapping.
/// </summary>
public class ShortVideoMessageReceivedXmlMapperProfile : Profile
{
    /// <summary>
    /// 
    /// </summary>
    public ShortVideoMessageReceivedXmlMapperProfile()
    {
        CreateMap<ShortVideoMessageReceivedXml, ShortVideoMessageReceivedEntry>(MemberList.Destination)
            .ReverseMap();
        CreateMap<ShortVideoMessageReceivedXml, WeixinReceivedMessageEntity>(MemberList.Destination);
    }
}
/// <summary>
/// Defines <see cref="VideoMessageReceivedXml"/> and <see cref="VideoMessageReceivedEntry"/> mapping.
/// </summary>
public class VideoMessageReceivedXmlMapperProfile : Profile
{
    /// <summary>
    /// 
    /// </summary>
    public VideoMessageReceivedXmlMapperProfile()
    {
        CreateMap<VideoMessageReceivedXml, VideoMessageReceivedEntry>(MemberList.Destination)
            .ReverseMap();
        CreateMap<VideoMessageReceivedXml, WeixinReceivedMessageEntity>(MemberList.Destination);
    }
}
/// <summary>
/// Defines <see cref="VoiceMessageReceivedXml"/> and <see cref="VoiceMessageReceivedEntry"/> mapping.
/// </summary>
public class VoiceMessageReceivedXmlMapperProfile : Profile
{
    /// <summary>
    /// 
    /// </summary>
    public VoiceMessageReceivedXmlMapperProfile()
    {
        CreateMap<VoiceMessageReceivedXml, VoiceMessageReceivedEntry>(MemberList.Destination)
            .ReverseMap();
        CreateMap<VoiceMessageReceivedXml, WeixinReceivedMessageEntity>(MemberList.Destination);
    }
}

/// <summary>
/// Defines <see cref="SubscribeEventReceivedXml"/> and <see cref="SubscribeEventReceivedEntry"/> mapping.
/// </summary>
public class SubscribeEventReceivedXmlMapperProfile : Profile
{
    /// <summary>
    /// 
    /// </summary>
    public SubscribeEventReceivedXmlMapperProfile()
    {
        CreateMap<SubscribeEventReceivedXml, SubscribeEventReceivedEntry>(MemberList.Destination)
            .ReverseMap();
        CreateMap<SubscribeEventReceivedXml, WeixinReceivedEventEntity>(MemberList.Destination);
    }
}

/// <summary>
/// Defines <see cref="UnsubscribeEventReceivedXml"/> and <see cref="UnsubscribeEventReceivedEntry"/> mapping.
/// </summary>
public class UnsubscribeEventReceivedXmlMapperProfile : Profile
{
    /// <summary>
    /// 
    /// </summary>
    public UnsubscribeEventReceivedXmlMapperProfile()
    {
        CreateMap<UnsubscribeEventReceivedXml, UnsubscribeEventReceivedEntry>(MemberList.Destination)
            .ReverseMap();
        CreateMap<UnsubscribeEventReceivedXml, WeixinReceivedEventEntity>(MemberList.Destination);
    }
}

/// <summary>
/// Defines <see cref="QrscanEventReceivedXml"/> and <see cref="QrscanEventReceivedEntry"/> mapping.
/// </summary>
public class QrscanEventReceivedXmlMapperProfile : Profile
{
    /// <summary>
    /// 
    /// </summary>
    public QrscanEventReceivedXmlMapperProfile()
    {
        CreateMap<QrscanEventReceivedXml, QrscanEventReceivedEntry>(MemberList.Destination)
            .ReverseMap();
        CreateMap<QrscanEventReceivedXml, WeixinReceivedEventEntity>(MemberList.Destination);
    }
}

/// <summary>
/// Defines <see cref="EnterEventReceivedXml"/> and <see cref="EnterEventReceivedEntry"/> mapping.
/// </summary>
public class EnterEventReceivedXmlMapperProfile : Profile
{
    /// <summary>
    /// 
    /// </summary>
    public EnterEventReceivedXmlMapperProfile()
    {
        CreateMap<EnterEventReceivedXml, EnterEventReceivedEntry>(MemberList.Destination)
            .ReverseMap();
        CreateMap<EnterEventReceivedXml, WeixinReceivedEventEntity>(MemberList.Destination);
    }
}

/// <summary>
/// Defines <see cref="LocationEventReceivedXml"/> and <see cref="LocationEventReceivedEntry"/> mapping.
/// </summary>
public class LocationEventReceivedXmlMapperProfile : Profile
{
    /// <summary>
    /// 
    /// </summary>
    public LocationEventReceivedXmlMapperProfile()
    {
        CreateMap<LocationEventReceivedXml, LocationEventReceivedEntry>(MemberList.Destination)
            .ReverseMap();
        CreateMap<LocationEventReceivedXml, WeixinReceivedEventEntity>(MemberList.Destination);
    }
}

/// <summary>
/// Defines <see cref="ClickMenuEventReceivedXml"/> and <see cref="ClickMenuEventReceivedEntry"/> mapping.
/// </summary>
public class ClickMenuEventReceivedXmlMapperProfile : Profile
{
    /// <summary>
    /// 
    /// </summary>
    public ClickMenuEventReceivedXmlMapperProfile()
    {
        CreateMap<ClickMenuEventReceivedXml, ClickMenuEventReceivedEntry>(MemberList.Destination)
            .ReverseMap();
        CreateMap<ClickMenuEventReceivedXml, WeixinReceivedEventEntity>(MemberList.Destination);
    }
}

/// <summary>
/// Defines <see cref="ViewMenuEventReceivedXml"/> and <see cref="ViewMenuEventReceivedEntry"/> mapping.
/// </summary>
public class ViewMenuEventReceivedXmlMapperProfile : Profile
{
    /// <summary>
    /// 
    /// </summary>
    public ViewMenuEventReceivedXmlMapperProfile()
    {
        CreateMap<ViewMenuEventReceivedXml, ViewMenuEventReceivedEntry>(MemberList.Destination)
            .ReverseMap();
        CreateMap<ViewMenuEventReceivedXml, WeixinReceivedEventEntity>(MemberList.Destination);
    }
}

