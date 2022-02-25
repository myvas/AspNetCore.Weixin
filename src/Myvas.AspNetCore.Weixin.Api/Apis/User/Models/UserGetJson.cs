using System.Collections.Generic;

namespace Myvas.AspNetCore.Weixin
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
    /// <remarks>https://developers.weixin.qq.com/doc/offiaccount/User_Management/Getting_a_User_List.html</remarks>
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
