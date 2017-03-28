using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetCore.Weixin
{
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

    /// <summary>
    /// 可下载的多媒体文件格式
    /// </summary>
    public enum DownloadMediaType
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
        /// 视频：10MB，支持MP4格式。不支持下载。
        /// </summary>
        /// <remarks>这里（<see cref="http://mp.weixin.qq.com/wiki/index.php?title=%E4%B8%8A%E4%BC%A0%E4%B8%8B%E8%BD%BD%E5%A4%9A%E5%AA%92%E4%BD%93%E6%96%87%E4%BB%B6"/>）说“视频文件不支持下载”，但经测试，微信手机发上来的视频是可以下载的。</remarks>
        video,
        /// <summary>
        /// thumb：64KB，支持JPG格式
        /// </summary>
        thumb
    }

    /// <summary>
    /// 上传媒体文件类型
    /// </summary>
    public enum MediaType
    {
        /// <summary>
        /// 图片: 1M，支持JPG格式
        /// </summary>
        image,
        /// <summary>
        /// 语音：2M，播放长度不超过60s，支持AMR\MP3格式
        /// </summary>
        voice,
        /// <summary>
        /// 视频：1MB，支持MP4格式
        /// </summary>
        video,
        /// <summary>
        /// thumb：64KB，支持JPG格式
        /// </summary>
        thumb,
        /// <summary>
        /// 图文消息
        /// </summary>
        news
    }
}
