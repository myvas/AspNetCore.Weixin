using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 本批次拿到的订阅者OpenId列表
/// </summary>
public class WeixinOpenIdListJson
{
    public List<string> openid { get; set; }
}

/// <summary>
/// 订阅者OpenId列表
/// </summary>
public class WeixinUserGetJson : WeixinErrorJson
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
    public WeixinOpenIdListJson data { get; set; }

    /// <summary>
    /// 本批次最后一个OpenId，作为下一次调用入参next_openid。
    /// </summary>
    public string next_openid { get; set; }

    [JsonIgnore]
    public override bool Succeeded => base.Succeeded && (next_openid != null);
}
