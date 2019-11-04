using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin
{
    //API：http://mp.weixin.qq.com/wiki/index.php?title=%E7%94%9F%E6%88%90%E5%B8%A6%E5%8F%82%E6%95%B0%E7%9A%84%E4%BA%8C%E7%BB%B4%E7%A0%81

    /// <summary>
    /// 二维码接口
    /// </summary>
    public static class QrCode
    {
        /// <summary>
        /// 创建临时/永久二维码（整型参数值）。
        /// </summary>
        /// <param name="expireSeconds">该二维码有效时间，以秒为单位。 最大不超过604800（即7天）。当值0时，将生成永久二维码</param>
        /// <param name="sceneId">场景值ID，临时二维码时为32位非0整型，永久二维码时最大值为100000</param>
        /// <returns></returns>
        public static async Task<CreateQrCodeResult> Create(string accessToken, int expireSeconds, int sceneId)
        {
            var urlFormat = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={0}";
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
            return await MessageSend.Send<CreateQrCodeResult>(accessToken, urlFormat, data);
        }

        /// <summary>
        /// 创建永久二维码（字符串参数值）
        /// </summary>
        /// <param name="expireSeconds">该二维码有效时间，以秒为单位。 最大不超过1800。0时为永久二维码</param>
        /// <param name="sceneId">场景值ID，临时二维码时为32位整型，永久二维码时最大值为1000</param>
        /// <returns></returns>
        public static async Task<CreateQrCodeResult> Create(string accessToken, string actionName, string sceneStr)
        {
            var urlFormat = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={0}";
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
            return await MessageSend.Send<CreateQrCodeResult>(accessToken, urlFormat, data);
        }

        /// <summary>
        /// 获取二维码（不需要AccessToken）
        /// 错误情况下（如ticket非法）返回HTTP错误码404。
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="stream">输出流</param>
        public static string ShowQrcode(string ticket)
        {
            var api = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=TICKET";
            api = api.Replace("TICKET", WebUtility.UrlEncode(ticket));
            return api;
        }

        /// <summary>
        /// 获取二维码（不需要AccessToken）
        /// 错误情况下（如ticket非法）返回HTTP错误码404。
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="stream">输出流</param>
        public static async Task ShowQrcode(string ticket, Stream stream)
        {
            var api = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=TICKET";
            api = api.Replace("TICKET", WebUtility.UrlEncode(ticket));
            await HttpUtility.Download(api, stream);
        }
    }
}
