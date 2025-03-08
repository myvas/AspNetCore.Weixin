namespace Myvas.AspNetCore.Weixin;

public class WeixinCustomerServiceMessageTypes
{
    public const string Text = "text";
    public const string Image = "image";
    public const string Voice = "voice";
    public const string Video = "video";
    public const string Music = "music";
    public const string News = "news";
    /// <summary>
    /// 调查问卷问题
    /// </summary>
    /// <example>您对本次服务是否满意呢？</example>
    public const string Question = "msgmenu";
    /// <summary>
    /// 微信卡券
    /// </summary>
    public const string Coupon = "wxcard";
    /// <summary>
    /// 小程序卡片
    /// </summary>
    public const string Widget = "miniprogrampage";
    /// <summary>
    /// 客服输入提醒
    /// </summary>
    public const string Typing = "Typing";
}
