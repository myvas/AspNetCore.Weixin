using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// 所有Request和Response消息的基类
    /// </summary>
    public class WeixinMessage
    {
        public string ToUserName { get; set; }
        public string FromUserName { get; set; }

        [XmlElement("CreateTime", Namespace = "")]
        public long CreateUnixTime { get; set; }
        [XmlIgnore]
        public DateTime CreateTime
        {
            get => new UnixDateTime(CreateUnixTime);
            set => CreateUnixTime = value.ToUnixTimeSeconds();
        }

        public WeixinMessage()
        {
            CreateTime = DateTime.Now;
        }
    }
}
