﻿using Newtonsoft.Json;
using System.Text.Json;

namespace Myvas.AspNetCore.Weixin
{

    /// <summary>
    /// 8.弹出地理位置选择器
    /// <para>用户点击按钮后，微信客户端将调起地理位置选择工具，
    /// 完成选择操作后，将选择的地理位置发送给开发者的服务器，
    /// 同时收起位置选择工具，随后可能会收到开发者下发的消息。</para>
    /// </summary>
    public class WeixinMenuItemLocationSelect : WeixinMenuItem, IWeixinMenuItemHasKey
    {
        public WeixinMenuItemLocationSelect()
        {
            Type = WeixinMenuItemTypes.LocationSelect;
        }

        /// <summary>
        /// 自定义事件码
        /// </summary>
        [JsonProperty("key")]
        public string Key { get; set; }
    }
}
