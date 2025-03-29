using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 视频消息
/// </summary>
[XmlRoot("xml", Namespace = "")]
public class VideoMessageReceivedXml : MessageReceivedXml
{
    /// <summary>
    /// 视频消息媒体id，可以调用获取临时素材接口拉取数据。
    /// </summary>
    public string MediaId { get; set; }

    /// <summary>
    /// 视频消息缩略图的媒体id，可以调用多媒体文件下载接口拉取数据。
    /// </summary>
    public string ThumbMediaId { get; set; }

}
