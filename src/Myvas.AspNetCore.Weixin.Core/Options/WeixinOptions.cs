namespace Myvas.AspNetCore.Weixin
{
    public class WeixinOptions
    {
        public WeixinAccessTokenOptions AccessToken { get; set; } = new WeixinAccessTokenOptions();
        public WeixinSiteOptions Site { get; set; } = new WeixinSiteOptions();
    }
}
