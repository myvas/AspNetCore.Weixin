using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetCore.Weixin
{
    public class GetQrcodeParam
    {
        /// <summary>
        /// Wi-Fi连接SSID，必须
        /// </summary>
        public string ssid { get; set; }

        /// <summary>
        /// 是否一次性二维码（多终端上网时生成的临时二维码需要传1），必须
        /// <example>
        /// <para>值域：</para>
        /// <para>qrtype=0 永久二维码</para>
        /// <para>qrtype=1 一次性二维码</para>
        /// </example>
        /// </summary>
        public int qrType { get; set; }

        /// <summary>
        /// 当pc设备获取动态二维码时，需要传入该参数
        /// </summary>
        public string lgnId { get; set; }

        /// <summary>
        /// 模板由官方指定，决定整个二维码标签图片的样式
        /// <example>
        /// <para>值域：</para>
        /// <para>0手机扫描二维码模版，不填默认为0</para>
        /// <para>100多终端扫描模版</para></example>
        /// </summary>
        public int templateId { get; set; }

        /// <summary>
        /// 背景图url （图片规格： 944*680px）
        /// </summary>
        /// <remarks>手机扫描二维码必须传入bgurl</remarks>
        public string bgUrl { get; set; }

        /// <summary>
        /// ssid对应的门店id
        /// </summary>
        public string storeId { get; set; }

        /// <summary>
        /// 二维码过期时间，格式是未来一个时间的秒数。
        /// <para>微信客户端在这个时间之后扫这个二维码，会给二维码失效的提示。</para>
        /// </summary>
        public int expireTime { get; set; }
    }
}
