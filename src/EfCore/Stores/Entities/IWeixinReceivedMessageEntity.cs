namespace Myvas.AspNetCore.Weixin;

public interface IWeixinReceivedMessageEntity : IEntity,
    IWeixinReceivedText,
    IWeixinReceivedImage,
    IWeixinReceivedVoice,
    IWeixinReceivedVoiceWithRecognition,
    IWeixinReceivedVideo,
    IWeixinReceivedShortVideo,
    IWeixinReceivedLink,
    IWeixinReceivedLocation
{
    string ConcurrencyStamp { get; set; }
}
