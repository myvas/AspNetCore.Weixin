namespace Myvas.AspNetCore.Weixin;

public class AdDetail_Template1 : AdDetail
{
    /// <summary>
    /// 文字广告位标题
    /// </summary>
    public string text;
    /// <summary>
    /// 文字广告位链接url
    /// </summary>
    public string textURL;

    /// <summary>
    /// 拒绝无参构造，以免用户忘记参数text和textUrl
    /// </summary>
    private AdDetail_Template1() { }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="text">附加文本广告。不能为空!</param>
    /// <param name="textUrl">附加文本广告URL。不能为空！</param>
    public AdDetail_Template1(string text, string textUrl)
    {
        if (string.IsNullOrEmpty(text)
            || string.IsNullOrEmpty(textUrl))
        {
            throw new WeixinException(109, "参数错误;text ad is empty", null);
        }

        this.text = text;
        this.textURL = textUrl;
    }
}
