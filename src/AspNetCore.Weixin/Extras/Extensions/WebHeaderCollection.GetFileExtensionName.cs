using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;

namespace AspNetCore.Weixin
{
    public static partial class WebHeaderCollectionExtensions
    {
    //    /// <summary>
    //    /// 从Web Headers中取出文件扩展名
    //    /// </summary>
    //    /// <param name="headers"></param>
    //    /// <returns></returns>
    //    public static string GetFileExtensionName(this WebHeaderCollection headers)
    //    {
    //        string key = "Content-Disposition";
    //        string contentDisposition = headers[key];
    //        if (contentDisposition != null)
    //        {
    //            var fileName = GetFileNameFromContentDisposition(contentDisposition);
    //            return Path.GetExtension(fileName);
    //        }
    //        return "";
    //    }
        /// <summary>
        /// 从Web Headers中取出文件扩展名
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static string GetFileExtensionName(this HttpHeaders headers)
        {
            string key = "Content-Disposition";
            var values = headers.GetValues(key);
            if (values != null)
            {
                string contentDisposition = values.ElementAtOrDefault(0);
                if (contentDisposition != null)
                {
                    var fileName = GetFileNameFromContentDisposition(contentDisposition);
                    return Path.GetExtension(fileName);
                }
            }
            return "";
        }

        /// <summary>
        /// 从Content-Disposition中取出文件名（filename）
        /// </summary>
        /// <param name="contentDisposition">Content-Disposition</param>
        /// <returns>文件名</returns>
        public static string GetFileNameFromContentDisposition(string contentDisposition)
        {
            if (contentDisposition == null) throw new ArgumentNullException("contentDisposition");

            const string contentFileNamePortion = "filename=";
            int fileNameStartIndex = contentDisposition.IndexOf(contentFileNamePortion, StringComparison.OrdinalIgnoreCase);
            fileNameStartIndex += contentFileNamePortion.Length;
            int originalFileNameLength = contentDisposition.Length - fileNameStartIndex;
            string originalFileName = contentDisposition.Substring(fileNameStartIndex, originalFileNameLength);
            originalFileName = originalFileName.Trim(new char[] { '\'', '"' });
            return originalFileName;
        }
    }
}
