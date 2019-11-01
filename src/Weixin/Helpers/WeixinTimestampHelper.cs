using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Myvas.AspNetCore.Weixin
{
    public static class WeixinTimestampHelper
    {
        /// <summary>
        /// Unix起始时间
        /// </summary>
        public static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// 微信使用北京时间（+8时区）
        /// </summary>
        public static readonly int WeixinTimeZone = +8;

        /// <summary>
        /// 将微信时间戳（整型数）转换为UTC时间
        /// </summary>
        /// <param name="weixinTimestamp">微信时间戳（整型数），与Unix时间戳一致</param>
        /// <returns>UTC时间</returns>
        /// <exception cref="ArgumentOutOfRangeException">微信时间戳少于基准时间</exception>
        public static DateTime ToUtcTime(long weixinTimestamp)
        {
            if (weixinTimestamp < 0) throw new ArgumentOutOfRangeException(nameof(weixinTimestamp));
            return UnixEpoch.AddSeconds(weixinTimestamp);
        }

        /// <summary>
        /// 将微信时间戳（整型数）转换为本地时间
        /// </summary>
        /// <param name="weixinTimestamp">微信时间戳（整型数）</param>
        /// <returns>本地时间</returns>
        public static DateTime ToLocalTime(long weixinTimestamp)
        {
            return ToUtcTime(weixinTimestamp).ToLocalTime();
        }

        /// <summary>
        /// 将微信时间戳（整型数字符串）转换为本地时间
        /// </summary>
        /// <param name="weixinTimestamp">微信时间戳（整型数字符串）</param>
        /// <returns>本地时间</returns>
        public static DateTime ToLocalTime(string weixinTimestamp)
        {
            if (long.TryParse(weixinTimestamp, out long timestamp))
            {
                return ToLocalTime(timestamp);
            }
            throw new ArgumentException(nameof(weixinTimestamp));
        }

        /// <summary>
        /// 转换为Unix时间(微信适用)
        /// </summary>
        /// <param name="beijingTime">本地时间（+8时区）</param>
        /// <returns>Unix时间(微信适用)，整型</returns>
        public static long FromBeijingTime(DateTime beijingTime)
        {
            var utcTime = new DateTime(beijingTime.AddHours(-WeixinTimeZone).Ticks, DateTimeKind.Utc);
            return FromUtcTime(utcTime);
        }

        /// <summary>
        /// 转换为Unix时间(微信适用)
        /// </summary>
        /// <param name="localTime">任何时间都行，不限定为北京时区</param>
        /// <returns>Unix时间(微信适用)，整型</returns>
        public static long FromLocalTime(DateTime localTime)
        {
            return FromUtcTime(localTime.ToUniversalTime());
        }

        public static long FromUtcTime(DateTime utcTime)
        {
            return (utcTime.Ticks - UnixEpoch.Ticks) / 10000000;
        }
    }
}
