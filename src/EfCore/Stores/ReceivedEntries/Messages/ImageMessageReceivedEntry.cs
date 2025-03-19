using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin.Models;

/// <summary>
/// 收到图片消息
/// </summary>
[XmlRoot("xml", Namespace = "")]
public class ImageMessageReceivedEntry : MessageReceivedEntry
{
    /// <summary>
    /// 图片链接（由系统生成）
    /// </summary>
    public string PicUrl { get; set; }

    /// <summary>
    /// 图片消息媒体id，可以调用获取临时素材接口拉取数据。
    /// </summary>
    public string MediaId { get; set; }
}
