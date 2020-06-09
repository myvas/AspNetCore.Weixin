using System;

namespace Myvas.AspNetCore.Weixin
{
    public class WeixinReceivedMessage : Entity,
        IWeixinReceivedText,
        IWeixinReceivedImage,
        IWeixinReceivedVoice,
        IWeixinReceivedVideo,
        IWeixinReceivedShortVideo,
        IWeixinReceivedLink,
        IWeixinReceivedLocation
    {
        public string FromUserName { get; set; }
        public string ToUserName { get; set; }
        public string CreateTime { get; set; }
        public DateTimeOffset? CreateTimeOffset
        {
            get { return WeixinTimestampHelper.ToLocalTime(CreateTime); }
        }
        public string MsgType { get; set; }

        public string MsgId { get; set; }
        public string Content { get; set; }
        public string PicUrl { get; set; }
        public string MediaId { get; set; }
        public string Format { get; set; }
        public string ThumbMediaId { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public decimal Scale { get; set; }
        public string Label { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
    }
}