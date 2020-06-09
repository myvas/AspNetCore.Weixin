using System;

namespace Myvas.AspNetCore.Weixin
{

    public interface IWeixinReceivedMessage : IWeixinReceived
    {
        string MsgId { get; set; }
    }
    public interface IWeixinReceivedText : IWeixinReceivedMessage
    {
        string Content { get; set; }
    }

    public interface IWeixinReceivedImage : IWeixinReceivedMessage
    {
        string PicUrl { get; set; }
        string MediaId { get; set; }
    }

    public interface IWeixinReceivedVoice: IWeixinReceivedMessage
    {
        string MediaId { get; set; }
        string Format { get; set; }
    }
    public interface IWeixinReceivedVoiceWithRecognition : IWeixinReceivedVoice
    {
        string Recognition { get; set; }
    }
    public interface IWeixinReceivedVideo : IWeixinReceivedMessage
    {
        string MediaId { get; set; }
        string ThumbMediaId { get; set; }
    }
    public interface IWeixinReceivedShortVideo : IWeixinReceivedMessage
    {
        string MediaId { get; set; }
        string ThumbMediaId { get; set; }
    }
    public interface IWeixinReceivedLocation : IWeixinReceivedMessage
    {
        decimal Longitude { get; set; }
        decimal Latitude { get; set; }
        decimal Scale { get; set; }
        string Label { get; set; }
    }
    public interface IWeixinReceivedLink : IWeixinReceivedMessage
    {
        string Title { get; set; }
        string Url { get; set; }
        string Description { get; set; }
    }
}