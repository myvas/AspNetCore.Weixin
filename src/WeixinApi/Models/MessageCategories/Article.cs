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
        /// 图片地址。支持JPG、PNG格式，较好的效果为大图360*200，小图200*200
        /// </summary>
        public string PicUrl { get; set; }
        /// <summary>
        /// 跳转链接地址
        /// </summary>
        public string Url { get; set; }
    }

}
