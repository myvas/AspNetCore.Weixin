using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCore.Weixin
{
    public enum MessageSendType
    {
        GET,
        POST
    }

    public static class MessageSend
    {
        public static async Task<WeixinErrorJson> Send(string accessToken, string urlFormat, object data, MessageSendType sendType = MessageSendType.POST)
        {
            return await Send<WeixinErrorJson>(accessToken, urlFormat, data, sendType);
        }

        public static async Task<T> Send<T>(string accessToken, string urlFormat, object data, MessageSendType sendType = MessageSendType.POST)
        {
            var url = string.IsNullOrEmpty(accessToken) ? urlFormat : string.Format(urlFormat, accessToken);
            switch (sendType)
            {
                case MessageSendType.GET:
                    return await HttpUtility.GetJson<T>(url);
                case MessageSendType.POST:
                    SerializerHelper serializer = new SerializerHelper();
                    var jsonString = serializer.GetJsonString(data);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        var bytes = Encoding.UTF8.GetBytes(jsonString);
                        ms.Write(bytes, 0, bytes.Length);
                        ms.Seek(0, SeekOrigin.Begin);

                        return await HttpUtility.PostGetJson<T>(url, null, ms);
                    }
                default:
                    throw new ArgumentOutOfRangeException("sendType");
            }
        }

        /// <summary>
        /// 简单地，通过GET请求方式，仅仅是传入一个URL(带参)，便可获取一个JSON对象，并将其反向序列化，最终得到一个简单数据实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<T> GetJson<T>(string url)
        {
            return await HttpUtility.GetJson<T>(url);
        }

        /// <summary>
        /// 通过POST请求方式，传入一个表单(object类型)，获取一个JSON对象，并将其反向序列化，最终得到一个简单数据实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="objRequest"></param>
        /// <returns></returns>
        public static async Task<T> GetJson<T>(string url, object objRequest)
        {
            SerializerHelper serializer = new SerializerHelper();
            var jsonString = serializer.GetJsonString(objRequest);
            using (MemoryStream ms = new MemoryStream())
            {
                var bytes = Encoding.UTF8.GetBytes(jsonString);
                ms.Write(bytes, 0, bytes.Length);
                ms.Seek(0, SeekOrigin.Begin);

                return await HttpUtility.PostGetJson<T>(url, null, ms);
            }
        }

        [Obsolete("该函数试图将完全不同的东西揉杂在一起，程序不健壮，对调用者不友好。建议换用两个重载的GetJson")]
        public static async Task<T> Send<T>(string url, object data, MessageSendType sendType = MessageSendType.GET)
        {
            switch (sendType)
            {
                case MessageSendType.GET:
                    return await HttpUtility.GetJson<T>(url);
                case MessageSendType.POST:
                    SerializerHelper serializer = new SerializerHelper();
                    var jsonString = serializer.GetJsonString(data);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        var bytes = Encoding.UTF8.GetBytes(jsonString);
                        ms.Write(bytes, 0, bytes.Length);
                        ms.Seek(0, SeekOrigin.Begin);

                        return await HttpUtility.PostGetJson<T>(url, null, ms);
                    }
                default:
                    throw new ArgumentOutOfRangeException("sendType");
            }
        }
    }
}
