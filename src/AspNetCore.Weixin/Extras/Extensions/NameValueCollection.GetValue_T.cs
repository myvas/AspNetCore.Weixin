using System;
using System.Collections.Specialized;
using System.ComponentModel;

namespace AspNetCore.Weixin
{
    public static partial class NameValueCollectionExtensions
    {
        /// <summary>
        /// 获取<see cref="NameValueCollection"/>的值。
        /// <para>若指定键不存在，则返回指定值。</para>
        /// </summary>
        /// <example>
        /// <code>
        /// Request.QueryString.GetValue&lt;int?>("id") ?? 10;
        /// Request.QueryString.GetValue&lt;int?>("id", 10);
        /// Request.QueryString.GetValue&lt;int>("id", 10);
        /// ConfigurationManager.AppSettings.GetValue&lt;string>("Token","Demo");
        /// </code></example>
        /// <typeparam name="T">参数值类型</typeparam>
        /// <param name="collection"></param>
        /// <param name="key">参数名</param>
        /// <returns>参数值。强类型。</returns>
        public static T GetValue<T>(this NameValueCollection collection, string key, T defaultValue = default(T))
        {
            if (collection == null)
            {
                return defaultValue;
            }

            var value = collection[key];
            if (value == null)
            {
                return defaultValue;
            }

            var type = typeof(T);
            if (type.IsConstructedGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                type = Nullable.GetUnderlyingType(type);
            }

            var converter = TypeDescriptor.GetConverter(type);
            if (!converter.CanConvertTo(value.GetType()))
            {
                return defaultValue;
            }

            try
            {
                //return (T)converter.ConvertTo(value, type);
                return (T)converter.ConvertFrom(value);
            }
            catch { return defaultValue; }
        }
    }
}
