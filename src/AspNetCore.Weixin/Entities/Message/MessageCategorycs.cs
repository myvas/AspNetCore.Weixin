using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// 微信图文混排
    /// </summary>
    public class Article
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string PicUrl { get; set; }
        /// <summary>
        /// 跳转链接地址
        /// </summary>
        public string Url { get; set; }
    }

    /// <summary>
    /// 图片资源
    /// </summary>
    public class Image
    {
        /// <summary>
        /// 资源号
        /// </summary>
        public string MediaId { get; set; }
    }

    /// <summary>
    /// 声音资源
    /// </summary>
    public class Voice
    {
        /// <summary>
        /// 资源号
        /// </summary>
        public string MediaId { get; set; }
    }

    /// <summary>
    /// 视频资源
    /// </summary>
    public class Video
    {
        /// <summary>
        /// 资源号
        /// </summary>
        public string MediaId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }

    /// <summary>
    /// 音乐资源
    /// </summary>
    public class Music
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 音乐地址
        /// </summary>
        public string MusicUrl { get; set; }
        /// <summary>
        /// 音乐地址（高质量版）
        /// </summary>
        public string HQMusicUrl { get; set; }
        ///// <summary>
        ///// 缩略图的媒体id，通过上传多媒体文件，得到的id
        ///// 官方API上有，但是加入的话会出错
        ///// </summary>
        //public string ThumbMediaId { get; set; }
    }

}
