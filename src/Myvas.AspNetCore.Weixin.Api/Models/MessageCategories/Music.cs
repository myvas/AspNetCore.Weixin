using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Myvas.AspNetCore.Weixin
{

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
