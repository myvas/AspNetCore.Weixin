using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetCore.Weixin
{
    public class RequestMessageText : RequestMessageBase, IRequestMessageBase
    {
        public override ReceivedMsgType MsgType
        {
            get { return ReceivedMsgType.text; }
        }
        public string Content { get; set; }
    }

    public class RequestMessageImage : RequestMessageBase, IRequestMessageBase
    {
        public override ReceivedMsgType MsgType
        {
            get { return ReceivedMsgType.image; }
        }

        public string MediaId { get; set; }
        public string PicUrl { get; set; }
    }


    public class RequestMessageLink : RequestMessageBase, IRequestMessageBase
    {
        public override ReceivedMsgType MsgType
        {
            get { return ReceivedMsgType.link; }
        }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
    }


    public class RequestMessageLocation : RequestMessageBase, IRequestMessageBase
    {
        public override ReceivedMsgType MsgType
        {
            get { return ReceivedMsgType.location; }
        }

        /// <summary>
        /// 地理位置纬度Location_X
        /// </summary>
        public decimal Latitude { get; set; }
        /// <summary>
        /// 地理位置经度Location_Y
        /// </summary>
        public decimal Longitude { get; set; }
        public decimal Scale { get; set; }
        public string Label { get; set; }
    }

    public class RequestMessageVoice : RequestMessageBase, IRequestMessageBase
    {
        public override ReceivedMsgType MsgType
        {
            get { return ReceivedMsgType.voice; }
        }

        public string MediaId { get; set; }
        /// <summary>
        /// 语音格式：amr
        /// </summary>
        public string Format { get; set; }
        /// <summary>
        /// 语音识别结果，UTF8编码
        /// 开通语音识别功能，用户每次发送语音给公众号时，微信会在推送的语音消息XML数据包中，增加一个Recongnition字段。
        /// 注：由于客户端缓存，开发者开启或者关闭语音识别功能，对新关注者立刻生效，对已关注用户需要24小时生效。开发者可以重新关注此帐号进行测试。
        /// </summary>
        public string Recognition { get; set; }
    }


    public class RequestMessageVideo : RequestMessageBase, IRequestMessageBase
    {
        public override ReceivedMsgType MsgType
        {
            get { return ReceivedMsgType.video; }
        }

        public string MediaId { get; set; }
        public string ThumbMediaId { get; set; }
    }


}
