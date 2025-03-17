using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Myvas.AspNetCore.Weixin
{
    public static class WeixinJsonHelper
    {
        /// <summary>
        /// unicode解码
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        private static string DecodeUnicode(Match match)
        {
            if (!match.Success)
            {
                return null;
            }

            char outStr = (char)int.Parse(match.Value.Remove(0, 2), NumberStyles.HexNumber);
            return new string(outStr, 1);
        }

        public static string Serialize(object data)
        {
            var jsonString = JsonSerializer.Serialize(data);

            var evaluator = new MatchEvaluator(DecodeUnicode);
            var json = Regex.Replace(jsonString, @"\\u[0123456789abcdef]{4}", evaluator);//或：[\\u007f-\\uffff]，\对应为\u000a，但一般情况下会保持\
            return json;
        }


        /// <summary>
        /// 获取Post结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonResult"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string jsonResult)
        {
            if (jsonResult.Contains("errcode"))
            {
                //可能发生错误
                WeixinErrorJson errorResult = JsonSerializer.Deserialize<WeixinErrorJson>(jsonResult);
                if (!errorResult.Succeeded)
                {
                    //发生错误
                    throw new WeixinException(
                        string.Format("微信Post请求发生错误！错误码：{0}，错误描述：{1}",
                                      errorResult.ErrorCode,
                                      errorResult.ErrorMessage),
                        null, errorResult);
                }
            }

            T result = JsonSerializer.Deserialize<T>(jsonResult);
            return result;
        }
    }
}
