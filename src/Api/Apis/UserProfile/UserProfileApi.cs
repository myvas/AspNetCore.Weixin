using Myvas.AspNetCore.Weixin.Helpers;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// 微信基础功能接口
    /// </summary>
    /// <remarks>https://developers.weixin.qq.com/doc/offiaccount/User_Management/Get_users_basic_information_UnionID.html</remarks>
    public class UserProfileApi : WeixinApiClient
    {
        public UserProfileApi(HttpClient client) : base(client)
        {
        }

        /// <summary>
        /// 用户信息接口
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openID"></param>
        /// <returns></returns>
        public async Task<UserInfoJson> GetUserInfo(string accessToken, string openID)
        {
            var url = string.Format(
                "http://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}",
                accessToken, openID);
            UserInfoJson result = await GetFromJsonAsync<UserInfoJson>(url);
            return result;
        }

        /// <summary>
        /// 媒体文件上传接口
        ///注意事项
        ///1.上传的媒体文件限制：
        ///图片（image) : 1MB，支持JPG格式
        ///语音（voice）：1MB，播放长度不超过60s，支持MP4格式
        ///视频（video）：10MB，支持MP4格式
        ///缩略图（thumb)：64KB，支持JPG格式
        ///2.媒体文件在后台保存时间为3天，即3天后media_id失效
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="UploadMediaType">上传文件类型</param>
        /// <param name="fileName">上传文件完整路径+文件名</param>
        /// <returns></returns>
        public async Task<UploadMediaResult> UploadMediaFile(string accessToken, MediaType type, string fileName)
        {
            var requestUriFormat = "http://api.weixin.qq.com/cgi-bin/media/upload?access_token={0}&UploadMediaType={1}&filename={2}&filelength={3}";
            var fileStream = FileStreamHelper.GetFileStream(fileName);

            var url = string.Format(requestUriFormat,
                accessToken, type.ToString(), Path.GetFileName(fileName), fileStream != null ? fileStream.Length : 0);
            UploadMediaResult result = await PostFileAsJsonAsync<UploadMediaResult>(url, fileStream);
            return result;
        }
    }
}
