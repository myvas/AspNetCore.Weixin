using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetCore.Weixin
{
    public static class DateTimeHelper
    {
        /// <summary>
        /// Unix起始时间
        /// </summary>
        public static DateTime BaseTime = new DateTime(1970, 1, 1);

        /// <summary>
        /// 微信使用本地时间（+8时区）
        /// </summary>
        public static int LocalTimeZone = +8;

        /// <summary>
        /// 将Unix时间（整型数）转换为本地时间（+8时区）
        /// </summary>
        /// <param name="dateTimeFromXml">Unix时间（整型数据）</param>
        /// <returns>本地时间（+8时区）</returns>
        public static DateTime ConvertDateTimeFromXml(long dateTimeFromXml)
        {
            return BaseTime.AddTicks((dateTimeFromXml + LocalTimeZone * 60 * 60) * 10000000);
        }
        /// <summary>
        /// 将Unix时间（整型数）字符串转换为本地时间（+8时区）
        /// </summary>
        /// <param name="dateTimeFromXml">Unix时间（整型数）字符串</param>
        /// <returns>本地时间（+8时区）</returns>
        public static DateTime ConvertDateTimeFromXml(string dateTimeFromXml)
        {
            return ConvertDateTimeFromXml(long.Parse(dateTimeFromXml));
        }

        /// <summary>
        /// 转换为Unix时间(微信适用)
        /// </summary>
        /// <param name="dateTime">本地时间（+8时区）</param>
        /// <returns>Unix时间(微信适用)，整型</returns>
        public static long ConvertToWeixinDateTime(DateTime dateTime)
        {
            return (dateTime.Ticks - BaseTime.Ticks) / 10000000 - LocalTimeZone * 60 * 60;
        }
    }
}
