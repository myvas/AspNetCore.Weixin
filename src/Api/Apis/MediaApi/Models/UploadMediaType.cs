namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 可上传的多媒体文件格式
/// </summary>
public enum UploadMediaType
{
    /// <summary>
    /// 图片: 1MB，支持JPG格式
    /// </summary>
    image,
    /// <summary>
    /// 语音：2MB，播放长度不超过60s，支持AMR\MP3格式
    /// </summary>
    voice,
    /// <summary>
    /// 视频：10MB，支持MP4格式
    /// </summary>
    video,
    /// <summary>
    /// thumb：64KB，支持JPG格式
    /// </summary>
    thumb
}
