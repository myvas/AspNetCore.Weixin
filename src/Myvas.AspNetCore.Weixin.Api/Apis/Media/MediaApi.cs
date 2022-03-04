﻿using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin
{

    /// <summary>
    /// 多媒体文件接口
    /// </summary>
    public class MediaApi : WeixinApiClient
    {
        public MediaApi(HttpClient client) : base(client)
        {
        }

        /// <summary>
        /// 上传媒体文件。
        /// 指定文件路径、文件格式和有效的访问令牌，将文件发送给微信服务器。
        /// 
        /// 注意：微信服务器只保存3天。
        /// </summary>
        /// <param name="accessToken">访问令牌</param>
        /// <param name="type">文件类型。支持JPG, MP3, MP4, AMR等格式，详见<see cref="UploadMediaType"/></param>
        /// <param name="file">文件路径</param>
        /// <returns>若成功，则返回上传成功的时间、文件格式、以及资源ID；若失败，则抛出异常。</returns>
        /// <exception cref="WeixinException">下载不成功，则抛出该异常</exception>
        public async Task<MediaUploadResultJson> Upload(string accessToken, UploadMediaType type, string file)
        {
            string api = "http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token=ACCESSTOKEN&UploadMediaType=UPLOADMEDIATYPE";
            api = api.Replace("ACCESSTOKEN", accessToken);
            api = api.Replace("UPLOADMEDIATYPE", type.ToString());

            //var fileDictionary = new Dictionary<string, string>();
            //fileDictionary["media"] = file;
            //return HttpUtilityPost.PostFileGetJson<MediaUploadResultJson>(url, null, fileDictionary, null);
            return await PostAsJsonAsync<string, MediaUploadResultJson>(api, file);
        }

        public async Task<MaterialCountJson> GetMaterialCount(string accessToken)
        {
            string api = "http://api.weixin.qq.com/cgi-bin/material/get_materialcount?access_token=ACCESSTOKEN";
            api = api.Replace("ACCESSTOKEN", accessToken);

            var response = await Http.GetAsync(api);
            if (!response.IsSuccessStatusCode)
            {
                //Logger.LogError($"An error occurred while retrieving the user profile: the remote server returned a {response.StatusCode} response with the following payload: {response.Headers.ToString()} {await response.Content.ReadAsStringAsync()}");
                throw new HttpRequestException("An error occured while retrieving the user profile.");
            }

            var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            int errorCode = WeixinApiHelper.GetErrorCode(payload);
            if (errorCode != 0)
            {
                var errorMessage = WeixinApiHelper.GetErrorMessage(payload);
                //Logger.LogError($"The remote server returned an error while retrieving the user profile. {errorCode} {errorMessage}");
                throw new Exception($"The remote server returned an error while retrieving the user profile. {errorCode} {errorMessage}");
            }


            int voiceCount, videoCount, imageCount, newsCount;
            if (MediaApiHelper.GetAllCounts(payload, out voiceCount, out videoCount, out imageCount, out newsCount))
            {
                return new MaterialCountJson
                {
                    VoiceCount = voiceCount,
                    VideoCount = videoCount,
                    ImageCount = imageCount,
                    NewsCount = newsCount
                };
            }
            throw new Exception($"The remote server returned an unknown formatted string while retrieving the count of materials.");
        }

        public async Task<BatchGetMaterialsJson> BatchGetMaterials(string accessToken, string materialType, int offset, int count)
        {
            string api = "http://api.weixin.qq.com/cgi-bin/material/batchget_material?access_token=ACCESSTOKEN";
            api = api.Replace("ACCESSTOKEN", accessToken);

            var content = new
            {
                type = materialType,
                offset = offset,
                count = count
            };
            return await PostAsJsonAsync<object, BatchGetMaterialsJson>(api, content);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="materialType">news, image, video, voice</param>
        /// <returns></returns>
        public async Task<BatchGetMaterialsJson> GetAllMaterialsAsync(string accessToken, MaterialType materialType)
        {
            var type = materialType.ToString();

            var result = new BatchGetMaterialsJson();

            var materialCount = await GetMaterialCount(accessToken);
            var newsCount = materialCount.NewsCount;


            int callLoopTimes = (int)(((double)newsCount) / 20);
            for (int i = 0; i < callLoopTimes; i++)
            {
                var partialResult = await BatchGetMaterials(accessToken, type, i * 20, 20);
                result.item.AddRange(partialResult.item);
            }

            result.total_count = newsCount;
            result.item_count = newsCount;
            return result;
        }

        /// <summary>
        /// 下载媒体文件。
        /// <para>下载成功后，将在指定位置创建一个文件。</para>
        /// </summary>
        /// <param name="accessToken">访问令牌</param>
        /// <param name="mediaId">媒体ID</param>
        /// <param name="filePath">文件路径。
        /// <para>路径首尾不要加斜杠(/)，或反斜杠(\）！</para>
        /// <para>若为空，则默认为根虚拟路径（~/）对应的物理路径。</para></param>
        /// <param name="folderName">文件夹名。
        /// <para>文件夹名首尾不要加斜杠(/)，或反斜杠(\）！</para></param>
        /// <param name="fileName">文件名。
        /// <para>若为空，则自动获取一个GUID作为文件名。</para></param>
        /// <param name="fileExtensionName">文件扩展名。
        /// <para>文件扩展名一般以点号(.)开始。</para>
        /// <para>若为空，则自动从服务器响应头中取得（Content-Disposition中的filename的扩展名）。</para></param>
        /// <returns>字符串。文件存放的物理路径及文件名（含文件扩展名）</returns>
        public async Task<string> DownloadFile(string accessToken, string mediaId, string filePath = "", string folderName = "MediaFiles", string fileName = "", string fileExtensionName = "")
        {
            // Download a media

            string api = "http://file.api.weixin.qq.com/cgi-bin/media/get?access_token=ACCESSTOKEN&media_id=MEDIAID";
            api = api.Replace("ACCESSTOKEN", accessToken);
            api = api.Replace("MEDIAID", mediaId);

            using (var response = await Http.GetAsync(api))
            using (Stream rawStream = await response.Content.ReadAsStreamAsync())
            {
                if (string.IsNullOrEmpty(filePath)) filePath = Directory.GetCurrentDirectory();
                if (string.IsNullOrEmpty(fileName)) fileName = Guid.NewGuid().ToString("N");
                if (string.IsNullOrEmpty(fileExtensionName))
                {
                    fileExtensionName = response.Headers.GetFileExtensionName();
                }
                string fullFileName = Path.Combine(filePath, folderName, fileName + fileExtensionName);

                using (FileStream fs = new FileStream(fullFileName, FileMode.Create))
                {
                    rawStream.CopyTo(fs);
                    return fullFileName;
                }
            }
        }

        /// <summary>
        /// 上传图文消息素材
        /// </summary>
        /// <param name="accessToken">Token</param>
        /// <param name="news">图文消息组</param>
        /// <returns></returns>
        public async Task<UploadMediaResult> UploadNews(string accessToken, params NewsModel[] news)
        {
            const string urlFormat = "https://api.weixin.qq.com/cgi-bin/media/uploadnews?access_token={0}";
            var data = new
            {
                articles = news
            };
            return await PostAsJsonAsync<object, UploadMediaResult>(accessToken, urlFormat, data);
        }
    }
}
