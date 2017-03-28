using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetCore.Weixin
{
    /// <summary>
    /// 本批次拿到的订阅者OpenId列表
    /// </summary>
    public class OpenIdListJson
    {
        public List<string> openid { get; set; }
    }

    /// <summary>
    /// 订阅者OpenId列表
    /// </summary>
    public class UserGetJson : WeixinErrorJson
    {
        /// <summary>
        /// 订阅者总数
        /// </summary>
       public int total { get; set; }

        /// <summary>
        /// 本批次拿到的订阅者数量
        /// </summary>
       public int count { get; set; }

        /// <summary>
        /// 本批次拿到的订阅者OpenId列表
        /// </summary>
       public OpenIdListJson data { get; set; }

        /// <summary>
        /// 下一批次将从此OpenId开始拿
        /// </summary>
       public string next_openid { get; set; }
    }

}
