using Newtonsoft.Json;
using System.Text.Json;

namespace Myvas.AspNetCore.Weixin
{

    /// <summary>
    /// 2.点击跳转网页
    /// <para>用户点击view类型按钮后，微信客户端将会打开开发者在按钮中填写的网页URL，
    /// 可与网页授权获取用户基本信息接口结合，获得用户基本信息。</para>
    /// </summary>
    public class WeixinMenuItemView : WeixinMenuItem, IWeixinMenuItemHasUrl
    {
        public WeixinMenuItemView()
        {
            Type = WeixinMenuItemTypes.View;
        }

        /// <summary>
        /// 网址 eg. https://myvas.com/go/to/a/absolute/url
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
