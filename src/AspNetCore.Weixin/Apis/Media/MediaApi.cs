using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace AspNetCore.Weixin
{
    public class MaterialCountJson
    {
        [JsonProperty("video_count")]
        public int VideoCount { get; set; }
        [JsonProperty("voice_count")]
        public int VoiceCount { get; set; }
        [JsonProperty("image_count")]
        public int ImageCount { get; set; }
        [JsonProperty("news_count")]
        public int NewsCount { get; set; }
    }
    public class BatchGetMaterialsJson
    {
        public int total_count { get; set; }
        public int item_count { get; set; }
        public List<MaterialItem> item { get; set; }

        public class MaterialItem
        {
            public string media_id { get; set; }
            public string update_time { get; set; }
        }
        public class NonNewsMaterialItem : MaterialItem
        {
            public string name { get; set; }
            public string url { get; set; }
        }
        public class NewsMaterialItem : MaterialItem
        {
            public NewsMaterialItemContent content { get; set; }
        }

        public class NewsMaterialItemContent
        {
            public List<NewsMaterialItemContentItem> news_item { get; set; }

        }

        public class NewsMaterialItemContentItem
        {
            public string title { get; set; }
            public string thumb_media_id { get; set; }
            public string show_cover_pic { get; set; }
            public string author { get; set; }
            public string digest { get; set; }
            public string content { get; set; }
            public string url { get; set; }
            public string content_source_url { get; set; }
        }

    }

    /// <summary>
    /// 多媒体文件接口
    /// </summary>
    public static class MediaApi
    {
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
        public static async Task<MediaUploadResultJson> Upload(string accessToken, UploadMediaType type, string file)
        {
            string api = "http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token=ACCESSTOKEN&UploadMediaType=UPLOADMEDIATYPE";
            api = api.Replace("ACCESSTOKEN", accessToken);
            api = api.Replace("UPLOADMEDIATYPE", type.ToString());

            //var fileDictionary = new Dictionary<string, string>();
            //fileDictionary["media"] = file;
            //return HttpUtilityPost.PostFileGetJson<MediaUploadResultJson>(url, null, fileDictionary, null);
            var httpResponseMessage = await HttpUtility.UploadFile(api, file);
            byte[] responseData = await httpResponseMessage.Content.ReadAsByteArrayAsync();
            string resp = Encoding.UTF8.GetString(responseData);

            var json = JsonConvert.DeserializeObject<MediaUploadResultJson>(resp);

            return json;
        }

        public static async Task<MaterialCountJson> GetMaterialCount(string accessToken)
        {
            string api = "http://api.weixin.qq.com/cgi-bin/material/get_materialcount?access_token=ACCESSTOKEN";
            api = api.Replace("ACCESSTOKEN", accessToken);

            var client = new HttpClient();
            var response = await client.GetAsync(api);
            if (!response.IsSuccessStatusCode)
            {
                //Logger.LogError($"An error occurred while retrieving the user profile: the remote server returned a {response.StatusCode} response with the following payload: {response.Headers.ToString()} {await response.Content.ReadAsStringAsync()}");
                throw new HttpRequestException("An error occured while retrieving the user profile.");
            }

            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());
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

        public static async Task<BatchGetMaterialsJson> BatchGetMaterials(string accessToken, string materialType, int offset, int count)
        {
            string api = "http://api.weixin.qq.com/cgi-bin/material/batchget_material?access_token=ACCESSTOKEN";
            api = api.Replace("ACCESSTOKEN", accessToken);

            var content = new
            {
                type = materialType,
                offset = offset,
                count = count
            }.ToJson();

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, api);
            requestMessage.Content = new StringContent(content);
            var client = new HttpClient();
            var response = await client.SendAsync(requestMessage);
            if (!response.IsSuccessStatusCode)
            {
                //Logger.LogError($"An error occurred while retrieving the user profile: the remote server returned a {response.StatusCode} response with the following payload: {response.Headers.ToString()} {await response.Content.ReadAsStringAsync()}");
                throw new HttpRequestException("An error occured while retrieving the user profile.");
            }

            var responseString = await response.Content.ReadAsStringAsync();
            var payload = JObject.Parse(responseString);
            int errorCode = WeixinApiHelper.GetErrorCode(payload);
            if (errorCode != 0)
            {
                var errorMessage = WeixinApiHelper.GetErrorMessage(payload);
                //Logger.LogError($"The remote server returned an error while retrieving the user profile. {errorCode} {errorMessage}");
                throw new Exception($"The remote server returned an error while retrieving the user profile. {errorCode} {errorMessage}");
            }
            var result = JsonConvert.DeserializeObject<BatchGetMaterialsJson>(responseString);
            return result;
        }

        public enum MaterialType
        {
            news,
            images,
            video,
            voice
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="materialType">news, image, video, voice</param>
        /// <returns></returns>
        public static async Task<BatchGetMaterialsJson> GetAllMaterialsAsync(string accessToken, MaterialType materialType)
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
        public static async Task<string> DownloadFile(string accessToken, string mediaId, string filePath = "", string folderName = "MediaFiles", string fileName = "", string fileExtensionName = "")
        {
            // Download a media

            string api = "http://file.api.weixin.qq.com/cgi-bin/media/get?access_token=ACCESSTOKEN&media_id=MEDIAID";
            api = api.Replace("ACCESSTOKEN", accessToken);
            api = api.Replace("MEDIAID", mediaId);

            using (var client = new HttpClient())
            using (var response = await client.GetAsync(api))
            using (Stream rawStream = await response.Content.ReadAsStreamAsync())
            {
                if (string.IsNullOrEmpty(filePath)) filePath = Directory.GetCurrentDirectory();
                if (string.IsNullOrEmpty(fileName)) fileName = Guid.NewGuid().ToString("N");
                if (string.IsNullOrEmpty(fileExtensionName)) fileExtensionName = response.Headers.GetFileExtensionName();
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
        public static async Task<UploadMediaResult> UploadNews(string accessToken, params NewsModel[] news)
        {
            const string urlFormat = "https://api.weixin.qq.com/cgi-bin/media/uploadnews?access_token={0}";
            var data = new
            {
                articles = news
            };
            return await MessageSend.Send<UploadMediaResult>(accessToken, urlFormat, data);
        }
    }
}
