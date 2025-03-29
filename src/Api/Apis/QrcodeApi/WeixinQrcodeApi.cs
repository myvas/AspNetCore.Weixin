using Microsoft.Extensions.Options;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 二维码接口
/// </summary>
/// <remarks>See: http://mp.weixin.qq.com/wiki/index.php?title=%E7%94%9F%E6%88%90%E5%B8%A6%E5%8F%82%E6%95%B0%E7%9A%84%E4%BA%8C%E7%BB%B4%E7%A0%81
/// </remarks>
public class WeixinQrcodeApi : WeixinSecureApiClient, IWeixinQrcodeApi
{
    public WeixinQrcodeApi(IOptions<WeixinOptions> optionsAccessor, IWeixinAccessTokenApi tokenProvider) : base(optionsAccessor, tokenProvider)
    {
    }

    /// <summary>
    /// 创建临时/永久二维码（整型参数值）。
    /// </summary>
    /// <param name="expireSeconds">该二维码有效时间，以秒为单位。 最大不超过604800（即7天）。当值0时，将生成永久二维码</param>
    /// <param name="sceneId">场景值ID，临时二维码时为32位非0整型，永久二维码时最大值为100000</param>
    /// <returns></returns>
    public async Task<WeixinCreateQrCodeResult> Create(int expireSeconds, int sceneId)
    {
        var pathAndQuery = "/cgi-bin/qrcode/create?access_token={0}";
        var url = Options?.BuildWeixinPlatformUrl(pathAndQuery);

        object data = null;
        if (expireSeconds > 0)
        {
            data = new
            {
                expire_seconds = expireSeconds,
                action_name = "QR_SCENE",
                action_info = new
                {
                    scene = new
                    {
                        scene_id = sceneId
                    }
                }
            };
        }
        else
        {
            data = new
            {
                action_name = "QR_LIMIT_SCENE",
                action_info = new
                {
                    scene = new
                    {
                        scene_id = sceneId
                    }
                }
            };
        }
        return await SecurePostAsJsonAsync<object, WeixinCreateQrCodeResult>(url, data);
    }

    /// <summary>
    /// 创建永久二维码（字符串参数值）
    /// </summary>
    /// <param name="expireSeconds">该二维码有效时间，以秒为单位。 最大不超过1800。0时为永久二维码</param>
    /// <param name="sceneId">场景值ID，临时二维码时为32位整型，永久二维码时最大值为1000</param>
    /// <returns></returns>
    public async Task<WeixinCreateQrCodeResult> Create(string actionName, string sceneStr, CancellationToken cancellationToken = default)
    {
        var pathAndQuery = "/cgi-bin/qrcode/create?access_token={0}";
        var url = Options?.BuildWeixinPlatformUrl(pathAndQuery);
        url = await FormatUrlWithTokenAsync(url, cancellationToken);

        object data = new
        {
            action_name = "QR_LIMIT_STR_SCENE",
            action_info = new
            {
                scene = new
                {
                    scene_str = sceneStr
                }
            }
        };

        return await PostAsJsonAsync<object, WeixinCreateQrCodeResult>(url, data);
    }

    /// <summary>
    /// 获取二维码（不需要AccessToken）
    /// 错误情况下（如ticket非法）返回HTTP错误码404。
    /// </summary>
    /// <param name="ticket"></param>
    /// <param name="stream">输出流</param>
    public string ShowQrcode(string ticket)
    {
        var pathAndQuery = "/cgi-bin/showqrcode?ticket=TICKET";
        var url = Options?.BuildWeixinPlatformUrl(pathAndQuery);
        url = url.Replace("TICKET", WebUtility.UrlEncode(ticket));
        return url;
    }

    /// <summary>
    /// 获取二维码（不需要AccessToken）
    /// 错误情况下（如ticket非法）返回HTTP错误码404。
    /// </summary>
    /// <param name="ticket"></param>
    /// <param name="stream">输出流</param>
    public async Task ShowQrcode(string ticket, Stream stream)
    {
        var pathAndQuery = "/cgi-bin/showqrcode?ticket=TICKET";
        var url = Options?.BuildWeixinPlatformUrl(pathAndQuery);
        url = url.Replace("TICKET", WebUtility.UrlEncode(ticket));

        await Download(url, stream);
    }
}
