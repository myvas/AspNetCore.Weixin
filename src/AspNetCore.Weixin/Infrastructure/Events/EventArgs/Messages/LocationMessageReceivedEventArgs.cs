using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AspNetCore.Weixin
{
    /// <summary>
    /// 收到地理位置消息
    /// </summary>
    [XmlRoot("xml", Namespace = "")]
    public class LocationMessageReceivedEventArgs : MessageReceivedEventArgs
    {
        /// <summary>
        /// Location_X, 纬度
        /// <example>23.134521</example>
        /// </summary>
        [XmlElement("Location_X", Namespace ="")]
        public decimal Latitude { get; set; }

        /// <summary>
        /// Location_Y, 经度
        /// <example>113.358803</example>
        /// </summary>
        [XmlElement("Location_Y", Namespace = "")]
        public decimal Longitude { get; set; }

        /// <summary>
        /// 缩放大小
        /// <example>20</example>
        /// <remarks>注意：与Google Maps不一致</remarks>
        /// </summary>
        public decimal Scale { get; set; }

        /// <summary>
        /// 地址标注
        /// </summary>
        public string Label { get; set; }
    }
}
