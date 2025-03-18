using AutoMapper;
using Myvas.AspNetCore.Weixin.Models;

namespace Myvas.AspNetCore.Weixin.EntityFrameworkCore;

/// <summary>
/// Extensions methods to map to/from xml/entity.
/// </summary>
public static class ReceivedXmlMappers
{
    internal static IMapper Mapper { get; }

    static ReceivedXmlMappers()
    {
        Mapper = new MapperConfiguration(x => x.AddProfile<ReceivedXmlMapperProfile>())
            .CreateMapper();
    }

    /// <summary>
    /// Maps an entity to a xml.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static ReceivedXml ToXml(this ReceivedEntry entity)
    {
        return entity == null ? null : Mapper.Map<ReceivedXml>(entity);
    }

    /// <summary>
    /// Maps a xml to an entity.
    /// </summary>
    /// <param name="xml"></param>
    /// <returns></returns>
    public static ReceivedEntry ToEntity(this ReceivedXml xml)
    {
        return xml == null ? null : Mapper.Map<ReceivedEntry>(xml);
    }

    public static WeixinReceivedMessage ToMessage(this ReceivedXml xml){
        return xml==null?null: Mapper.Map<WeixinReceivedMessage>(xml);
    }

    /// <summary>
    /// Updates an entity from a xml.
    /// </summary>
    /// <param name="xml"></param>
    /// <param name="entity"></param>
    public static void UpdateEntity(this ReceivedXml xml, ReceivedEntry entity)
    {
        Mapper.Map(xml, entity);
    }
}


/// <summary>
/// Extensions methods to map to/from xml/entity.
/// </summary>
public static class MessageReceivedXmlMappers
{
    internal static IMapper Mapper { get; }

    static MessageReceivedXmlMappers()
    {
        Mapper = new MapperConfiguration(x => x.AddProfile<MessageReceivedXmlMapperProfile>())
            .CreateMapper();
    }

    /// <summary>
    /// Maps an entity to a xml.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static MessageReceivedXml ToXml(this MessageReceivedEntry entity)
    {
        return entity == null ? null : Mapper.Map<MessageReceivedXml>(entity);
    }

    /// <summary>
    /// Maps a xml to an entity.
    /// </summary>
    /// <param name="xml"></param>
    /// <returns></returns>
    public static MessageReceivedEntry ToEntity(this MessageReceivedXml xml)
    {
        return xml == null ? null : Mapper.Map<MessageReceivedEntry>(xml);
    }

    public static WeixinReceivedMessage ToMessage(this MessageReceivedXml xml){
        return xml==null?null: Mapper.Map<WeixinReceivedMessage>(xml);
    }

    /// <summary>
    /// Updates an entity from a xml.
    /// </summary>
    /// <param name="xml"></param>
    /// <param name="entity"></param>
    public static void UpdateEntity(this MessageReceivedXml xml, MessageReceivedEntry entity)
    {
        Mapper.Map(xml, entity);
    }
}

/// <summary>
/// Extensions methods to map to/from xml/entity.
/// </summary>
public static class EventReceivedXmlMappers
{
    internal static IMapper Mapper { get; }

    static EventReceivedXmlMappers()
    {
        Mapper = new MapperConfiguration(x => x.AddProfile<EventReceivedXmlMapperProfile>())
            .CreateMapper();
    }

    /// <summary>
    /// Maps an entity to a xml.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static EventReceivedXml ToXml(this EventReceivedEntry entity)
    {
        return entity == null ? null : Mapper.Map<EventReceivedXml>(entity);
    }

    /// <summary>
    /// Maps a xml to an entity.
    /// </summary>
    /// <param name="xml"></param>
    /// <returns></returns>
    public static EventReceivedEntry ToEntity(this EventReceivedXml xml)
    {
        return xml == null ? null : Mapper.Map<EventReceivedEntry>(xml);
    }

    public static WeixinReceivedMessage ToMessage(this EventReceivedXml xml){
        return xml==null?null: Mapper.Map<WeixinReceivedMessage>(xml);
    }

    /// <summary>
    /// Updates an entity from a xml.
    /// </summary>
    /// <param name="xml"></param>
    /// <param name="entity"></param>
    public static void UpdateEntity(this EventReceivedXml xml, EventReceivedEntry entity)
    {
        Mapper.Map(xml, entity);
    }
}


/// <summary>
/// Extensions methods to map to/from xml/entity.
/// </summary>
public static class TextMessageReceivedXmlMappers
{
    internal static IMapper Mapper { get; }

    static TextMessageReceivedXmlMappers()
    {
        Mapper = new MapperConfiguration(x => x.AddProfile<TextMessageReceivedXmlMapperProfile>())
            .CreateMapper();
    }

    /// <summary>
    /// Maps an entity to a xml.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static TextMessageReceivedXml ToXml(this TextMessageReceivedEntry entity)
    {
        return entity == null ? null : Mapper.Map<TextMessageReceivedXml>(entity);
    }

    /// <summary>
    /// Maps a xml to an entity.
    /// </summary>
    /// <param name="xml"></param>
    /// <returns></returns>
    public static TextMessageReceivedEntry ToEntity(this TextMessageReceivedXml xml)
    {
        return xml == null ? null : Mapper.Map<TextMessageReceivedEntry>(xml);
    }

    public static WeixinReceivedMessage ToMessage(this TextMessageReceivedXml xml){
        return xml==null?null: Mapper.Map<WeixinReceivedMessage>(xml);
    }

    /// <summary>
    /// Updates an entity from a xml.
    /// </summary>
    /// <param name="xml"></param>
    /// <param name="entity"></param>
    public static void UpdateEntity(this TextMessageReceivedXml xml, TextMessageReceivedEntry entity)
    {
        Mapper.Map(xml, entity);
    }
}
/// <summary>
/// Extensions methods to map to/from xml/entity.
/// </summary>
public static class ImageMessageReceivedXmlMappers
{
    internal static IMapper Mapper { get; }

    static ImageMessageReceivedXmlMappers()
    {
        Mapper = new MapperConfiguration(x => x.AddProfile<ImageMessageReceivedXmlMapperProfile>())
            .CreateMapper();
    }

    /// <summary>
    /// Maps an entity to a xml.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static ImageMessageReceivedXml ToXml(this ImageMessageReceivedEntry entity)
    {
        return entity == null ? null : Mapper.Map<ImageMessageReceivedXml>(entity);
    }

    /// <summary>
    /// Maps a xml to an entity.
    /// </summary>
    /// <param name="xml"></param>
    /// <returns></returns>
    public static ImageMessageReceivedEntry ToEntity(this ImageMessageReceivedXml xml)
    {
        return xml == null ? null : Mapper.Map<ImageMessageReceivedEntry>(xml);
    }

    public static WeixinReceivedMessage ToMessage(this ImageMessageReceivedXml xml){
        return xml==null?null: Mapper.Map<WeixinReceivedMessage>(xml);
    }

    /// <summary>
    /// Updates an entity from a xml.
    /// </summary>
    /// <param name="xml"></param>
    /// <param name="entity"></param>
    public static void UpdateEntity(this ImageMessageReceivedXml xml, ImageMessageReceivedEntry entity)
    {
        Mapper.Map(xml, entity);
    }
}
/// <summary>
/// Extensions methods to map to/from xml/entity.
/// </summary>
public static class LinkMessageReceivedXmlMappers
{
    internal static IMapper Mapper { get; }

    static LinkMessageReceivedXmlMappers()
    {
        Mapper = new MapperConfiguration(x => x.AddProfile<LinkMessageReceivedXmlMapperProfile>())
            .CreateMapper();
    }

    /// <summary>
    /// Maps an entity to a xml.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static LinkMessageReceivedXml ToXml(this LinkMessageReceivedEntry entity)
    {
        return entity == null ? null : Mapper.Map<LinkMessageReceivedXml>(entity);
    }

    /// <summary>
    /// Maps a xml to an entity.
    /// </summary>
    /// <param name="xml"></param>
    /// <returns></returns>
    public static LinkMessageReceivedEntry ToEntity(this LinkMessageReceivedXml xml)
    {
        return xml == null ? null : Mapper.Map<LinkMessageReceivedEntry>(xml);
    }

    public static WeixinReceivedMessage ToMessage(this LinkMessageReceivedXml xml){
        return xml==null?null: Mapper.Map<WeixinReceivedMessage>(xml);
    }

    /// <summary>
    /// Updates an entity from a xml.
    /// </summary>
    /// <param name="xml"></param>
    /// <param name="entity"></param>
    public static void UpdateEntity(this LinkMessageReceivedXml xml, LinkMessageReceivedEntry entity)
    {
        Mapper.Map(xml, entity);
    }
}
/// <summary>
/// Extensions methods to map to/from xml/entity.
/// </summary>
public static class LocationMessageReceivedXmlMappers
{
    internal static IMapper Mapper { get; }

    static LocationMessageReceivedXmlMappers()
    {
        Mapper = new MapperConfiguration(x => x.AddProfile<LocationMessageReceivedXmlMapperProfile>())
            .CreateMapper();
    }

    /// <summary>
    /// Maps an entity to a xml.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static LocationMessageReceivedXml ToXml(this LocationMessageReceivedEntry entity)
    {
        return entity == null ? null : Mapper.Map<LocationMessageReceivedXml>(entity);
    }

    /// <summary>
    /// Maps a xml to an entity.
    /// </summary>
    /// <param name="xml"></param>
    /// <returns></returns>
    public static LocationMessageReceivedEntry ToEntity(this LocationMessageReceivedXml xml)
    {
        return xml == null ? null : Mapper.Map<LocationMessageReceivedEntry>(xml);
    }

    public static WeixinReceivedMessage ToMessage(this LocationMessageReceivedXml xml){
        return xml==null?null: Mapper.Map<WeixinReceivedMessage>(xml);
    }

    /// <summary>
    /// Updates an entity from a xml.
    /// </summary>
    /// <param name="xml"></param>
    /// <param name="entity"></param>
    public static void UpdateEntity(this LocationMessageReceivedXml xml, LocationMessageReceivedEntry entity)
    {
        Mapper.Map(xml, entity);
    }
}
/// <summary>
/// Extensions methods to map to/from xml/entity.
/// </summary>
public static class VoiceMessageReceivedXmlMappers
{
    internal static IMapper Mapper { get; }

    static VoiceMessageReceivedXmlMappers()
    {
        Mapper = new MapperConfiguration(x => x.AddProfile<VoiceMessageReceivedXmlMapperProfile>())
            .CreateMapper();
    }

    /// <summary>
    /// Maps an entity to a xml.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static VoiceMessageReceivedXml ToXml(this VoiceMessageReceivedEntry entity)
    {
        return entity == null ? null : Mapper.Map<VoiceMessageReceivedXml>(entity);
    }

    /// <summary>
    /// Maps a xml to an entity.
    /// </summary>
    /// <param name="xml"></param>
    /// <returns></returns>
    public static VoiceMessageReceivedEntry ToEntity(this VoiceMessageReceivedXml xml)
    {
        return xml == null ? null : Mapper.Map<VoiceMessageReceivedEntry>(xml);
    }

    public static WeixinReceivedMessage ToMessage(this VoiceMessageReceivedXml xml){
        return xml==null?null: Mapper.Map<WeixinReceivedMessage>(xml);
    }

    /// <summary>
    /// Updates an entity from a xml.
    /// </summary>
    /// <param name="xml"></param>
    /// <param name="entity"></param>
    public static void UpdateEntity(this VoiceMessageReceivedXml xml, VoiceMessageReceivedEntry entity)
    {
        Mapper.Map(xml, entity);
    }
}

/// <summary>
/// Extensions methods to map to/from xml/entity.
/// </summary>
public static class VideoMessageReceivedXmlMappers
{
    internal static IMapper Mapper { get; }

    static VideoMessageReceivedXmlMappers()
    {
        Mapper = new MapperConfiguration(x => x.AddProfile<VideoMessageReceivedXmlMapperProfile>())
            .CreateMapper();
    }

    /// <summary>
    /// Maps an entity to a xml.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static VideoMessageReceivedXml ToXml(this VideoMessageReceivedEntry entity)
    {
        return entity == null ? null : Mapper.Map<VideoMessageReceivedXml>(entity);
    }

    /// <summary>
    /// Maps a xml to an entity.
    /// </summary>
    /// <param name="xml"></param>
    /// <returns></returns>
    public static VideoMessageReceivedEntry ToEntity(this VideoMessageReceivedXml xml)
    {
        return xml == null ? null : Mapper.Map<VideoMessageReceivedEntry>(xml);
    }

    public static WeixinReceivedMessage ToMessage(this VideoMessageReceivedXml xml){
        return xml==null?null: Mapper.Map<WeixinReceivedMessage>(xml);
    }

    /// <summary>
    /// Updates an entity from a xml.
    /// </summary>
    /// <param name="xml"></param>
    /// <param name="entity"></param>
    public static void UpdateEntity(this VideoMessageReceivedXml xml, VideoMessageReceivedEntry entity)
    {
        Mapper.Map(xml, entity);
    }
}
/// <summary>
/// Extensions methods to map to/from xml/entity.
/// </summary>
public static class ShortVideoMessageReceivedXmlMappers
{
    internal static IMapper Mapper { get; }

    static ShortVideoMessageReceivedXmlMappers()
    {
        Mapper = new MapperConfiguration(x => x.AddProfile<ShortVideoMessageReceivedXmlMapperProfile>())
            .CreateMapper();
    }

    /// <summary>
    /// Maps an entity to a xml.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static ShortVideoMessageReceivedXml ToXml(this ShortVideoMessageReceivedEntry entity)
    {
        return entity == null ? null : Mapper.Map<ShortVideoMessageReceivedXml>(entity);
    }

    /// <summary>
    /// Maps a xml to an entity.
    /// </summary>
    /// <param name="xml"></param>
    /// <returns></returns>
    public static ShortVideoMessageReceivedEntry ToEntity(this ShortVideoMessageReceivedXml xml)
    {
        return xml == null ? null : Mapper.Map<ShortVideoMessageReceivedEntry>(xml);
    }

    public static WeixinReceivedMessage ToMessage(this ShortVideoMessageReceivedXml xml){
        return xml==null?null: Mapper.Map<WeixinReceivedMessage>(xml);
    }

    /// <summary>
    /// Updates an entity from a xml.
    /// </summary>
    /// <param name="xml"></param>
    /// <param name="entity"></param>
    public static void UpdateEntity(this ShortVideoMessageReceivedXml xml, ShortVideoMessageReceivedEntry entity)
    {
        Mapper.Map(xml, entity);
    }
}


/// <summary>
/// Extensions methods to map to/from xml/entity.
/// </summary>
public static class SubscribeEventReceivedXmlMappers
{
    internal static IMapper Mapper { get; }

    static SubscribeEventReceivedXmlMappers()
    {
        Mapper = new MapperConfiguration(x => x.AddProfile<SubscribeEventReceivedXmlMapperProfile>())
            .CreateMapper();
    }

    /// <summary>
    /// Maps an entity to a xml.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static SubscribeEventReceivedXml ToXml(this SubscribeEventReceivedEntry entity)
    {
        return entity == null ? null : Mapper.Map<SubscribeEventReceivedXml>(entity);
    }

    /// <summary>
    /// Maps a xml to an entity.
    /// </summary>
    /// <param name="xml"></param>
    /// <returns></returns>
    public static SubscribeEventReceivedEntry ToEntity(this SubscribeEventReceivedXml xml)
    {
        return xml == null ? null : Mapper.Map<SubscribeEventReceivedEntry>(xml);
    }

    public static WeixinReceivedEvent ToEvent(this SubscribeEventReceivedXml xml){
        return xml==null?null: Mapper.Map<WeixinReceivedEvent>(xml);
    }

    /// <summary>
    /// Updates an entity from a xml.
    /// </summary>
    /// <param name="xml"></param>
    /// <param name="entity"></param>
    public static void UpdateEntity(this SubscribeEventReceivedXml xml, SubscribeEventReceivedEntry entity)
    {
        Mapper.Map(xml, entity);
    }
}
/// <summary>
/// Extensions methods to map to/from xml/entity.
/// </summary>
public static class UnsubscribeEventReceivedXmlMappers
{
    internal static IMapper Mapper { get; }

    static UnsubscribeEventReceivedXmlMappers()
    {
        Mapper = new MapperConfiguration(x => x.AddProfile<UnsubscribeEventReceivedXmlMapperProfile>())
            .CreateMapper();
    }

    /// <summary>
    /// Maps an entity to a xml.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static UnsubscribeEventReceivedXml ToXml(this UnsubscribeEventReceivedEntry entity)
    {
        return entity == null ? null : Mapper.Map<UnsubscribeEventReceivedXml>(entity);
    }

    /// <summary>
    /// Maps a xml to an entity.
    /// </summary>
    /// <param name="xml"></param>
    /// <returns></returns>
    public static UnsubscribeEventReceivedEntry ToEntity(this UnsubscribeEventReceivedXml xml)
    {
        return xml == null ? null : Mapper.Map<UnsubscribeEventReceivedEntry>(xml);
    }

    public static WeixinReceivedEvent ToEvent(this UnsubscribeEventReceivedXml xml){
        return xml==null?null: Mapper.Map<WeixinReceivedEvent>(xml);
    }

    /// <summary>
    /// Updates an entity from a xml.
    /// </summary>
    /// <param name="xml"></param>
    /// <param name="entity"></param>
    public static void UpdateEntity(this UnsubscribeEventReceivedXml xml, UnsubscribeEventReceivedEntry entity)
    {
        Mapper.Map(xml, entity);
    }
}

/// <summary>
/// Extensions methods to map to/from xml/entity.
/// </summary>
public static class EnterEventReceivedXmlMappers
{
    internal static IMapper Mapper { get; }

    static EnterEventReceivedXmlMappers()
    {
        Mapper = new MapperConfiguration(x => x.AddProfile<EnterEventReceivedXmlMapperProfile>())
            .CreateMapper();
    }

    /// <summary>
    /// Maps an entity to a xml.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static EnterEventReceivedXml ToXml(this EnterEventReceivedEntry entity)
    {
        return entity == null ? null : Mapper.Map<EnterEventReceivedXml>(entity);
    }

    /// <summary>
    /// Maps a xml to an entity.
    /// </summary>
    /// <param name="xml"></param>
    /// <returns></returns>
    public static EnterEventReceivedEntry ToEntity(this EnterEventReceivedXml xml)
    {
        return xml == null ? null : Mapper.Map<EnterEventReceivedEntry>(xml);
    }

    public static WeixinReceivedEvent ToEvent(this EnterEventReceivedXml xml){
        return xml==null?null: Mapper.Map<WeixinReceivedEvent>(xml);
    }

    /// <summary>
    /// Updates an entity from a xml.
    /// </summary>
    /// <param name="xml"></param>
    /// <param name="entity"></param>
    public static void UpdateEntity(this EnterEventReceivedXml xml, EnterEventReceivedEntry entity)
    {
        Mapper.Map(xml, entity);
    }
}

/// <summary>
/// Extensions methods to map to/from xml/entity.
/// </summary>
public static class LocationEventReceivedXmlMappers
{
    internal static IMapper Mapper { get; }

    static LocationEventReceivedXmlMappers()
    {
        Mapper = new MapperConfiguration(x => x.AddProfile<LocationEventReceivedXmlMapperProfile>())
            .CreateMapper();
    }

    /// <summary>
    /// Maps an entity to a xml.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static LocationEventReceivedXml ToXml(this LocationEventReceivedEntry entity)
    {
        return entity == null ? null : Mapper.Map<LocationEventReceivedXml>(entity);
    }

    /// <summary>
    /// Maps a xml to an entity.
    /// </summary>
    /// <param name="xml"></param>
    /// <returns></returns>
    public static LocationEventReceivedEntry ToEntity(this LocationEventReceivedXml xml)
    {
        return xml == null ? null : Mapper.Map<LocationEventReceivedEntry>(xml);
    }

    public static WeixinReceivedEvent ToEvent(this LocationEventReceivedXml xml){
        return xml==null?null: Mapper.Map<WeixinReceivedEvent>(xml);
    }

    /// <summary>
    /// Updates an entity from a xml.
    /// </summary>
    /// <param name="xml"></param>
    /// <param name="entity"></param>
    public static void UpdateEntity(this LocationEventReceivedXml xml, LocationEventReceivedEntry entity)
    {
        Mapper.Map(xml, entity);
    }
}
/// <summary>
/// Extensions methods to map to/from xml/entity.
/// </summary>
public static class ClickMenuEventReceivedXmlMappers
{
    internal static IMapper Mapper { get; }

    static ClickMenuEventReceivedXmlMappers()
    {
        Mapper = new MapperConfiguration(x => x.AddProfile<ClickMenuEventReceivedXmlMapperProfile>())
            .CreateMapper();
    }

    /// <summary>
    /// Maps an entity to a xml.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static ClickMenuEventReceivedXml ToXml(this ClickMenuEventReceivedEntry entity)
    {
        return entity == null ? null : Mapper.Map<ClickMenuEventReceivedXml>(entity);
    }

    /// <summary>
    /// Maps a xml to an entity.
    /// </summary>
    /// <param name="xml"></param>
    /// <returns></returns>
    public static ClickMenuEventReceivedEntry ToEntity(this ClickMenuEventReceivedXml xml)
    {
        return xml == null ? null : Mapper.Map<ClickMenuEventReceivedEntry>(xml);
    }

    public static WeixinReceivedEvent ToEvent(this ClickMenuEventReceivedXml xml){
        return xml==null?null: Mapper.Map<WeixinReceivedEvent>(xml);
    }

    /// <summary>
    /// Updates an entity from a xml.
    /// </summary>
    /// <param name="xml"></param>
    /// <param name="entity"></param>
    public static void UpdateEntity(this ClickMenuEventReceivedXml xml, ClickMenuEventReceivedEntry entity)
    {
        Mapper.Map(xml, entity);
    }
}
/// <summary>
/// Extensions methods to map to/from xml/entity.
/// </summary>
public static class ViewMenuEventReceivedXmlMappers
{
    internal static IMapper Mapper { get; }

    static ViewMenuEventReceivedXmlMappers()
    {
        Mapper = new MapperConfiguration(x => x.AddProfile<ViewMenuEventReceivedXmlMapperProfile>())
            .CreateMapper();
    }

    /// <summary>
    /// Maps an entity to a xml.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static ViewMenuEventReceivedXml ToXml(this ViewMenuEventReceivedEntry entity)
    {
        return entity == null ? null : Mapper.Map<ViewMenuEventReceivedXml>(entity);
    }

    /// <summary>
    /// Maps a xml to an entity.
    /// </summary>
    /// <param name="xml"></param>
    /// <returns></returns>
    public static ViewMenuEventReceivedEntry ToEntity(this ViewMenuEventReceivedXml xml)
    {
        return xml == null ? null : Mapper.Map<ViewMenuEventReceivedEntry>(xml);
    }

    public static WeixinReceivedEvent ToEvent(this ViewMenuEventReceivedXml xml){
        return xml==null?null: Mapper.Map<WeixinReceivedEvent>(xml);
    }

    /// <summary>
    /// Updates an entity from a xml.
    /// </summary>
    /// <param name="xml"></param>
    /// <param name="entity"></param>
    public static void UpdateEntity(this ViewMenuEventReceivedXml xml, ViewMenuEventReceivedEntry entity)
    {
        Mapper.Map(xml, entity);
    }
}
/// <summary>
/// Extensions methods to map to/from xml/entity.
/// </summary>
public static class QrscanEventReceivedXmlMappers
{
    internal static IMapper Mapper { get; }

    static QrscanEventReceivedXmlMappers()
    {
        Mapper = new MapperConfiguration(x => x.AddProfile<QrscanEventReceivedXmlMapperProfile>())
            .CreateMapper();
    }

    /// <summary>
    /// Maps an entity to a xml.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static QrscanEventReceivedXml ToXml(this QrscanEventReceivedEntry entity)
    {
        return entity == null ? null : Mapper.Map<QrscanEventReceivedXml>(entity);
    }

    /// <summary>
    /// Maps a xml to an entity.
    /// </summary>
    /// <param name="xml"></param>
    /// <returns></returns>
    public static QrscanEventReceivedEntry ToEntity(this QrscanEventReceivedXml xml)
    {
        return xml == null ? null : Mapper.Map<QrscanEventReceivedEntry>(xml);
    }

    public static WeixinReceivedEvent ToEvent(this QrscanEventReceivedXml xml){
        return xml==null?null: Mapper.Map<WeixinReceivedEvent>(xml);
    }

    /// <summary>
    /// Updates an entity from a xml.
    /// </summary>
    /// <param name="xml"></param>
    /// <param name="entity"></param>
    public static void UpdateEntity(this QrscanEventReceivedXml xml, QrscanEventReceivedEntry entity)
    {
        Mapper.Map(xml, entity);
    }
}