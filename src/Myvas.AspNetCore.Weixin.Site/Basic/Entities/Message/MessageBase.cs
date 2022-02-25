﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin
{
    public interface IMessageBase
    {
        string ToUserName { get; set; }
        string FromUserName { get; set; }
        DateTime CreateTime { get; set; }
    }

    /// <summary>
    /// 所有Request和Response消息的基类
    /// </summary>
    public class MessageBase
    {
        public string ToUserName { get; set; }
        public string FromUserName { get; set; }

        [XmlElement("CreateTime", Namespace = "")]
        public long CreateTimestamp { get; set; }
        [XmlIgnore]
        public DateTime CreateTime
        {
            get => WeixinTimestampHelper.ToLocalTime(CreateTimestamp);
            set => CreateTimestamp = WeixinTimestampHelper.FromLocalTime(value);
        }

        public MessageBase()
        {
            CreateTime = DateTime.Now;
        }
    }
}
