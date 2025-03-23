using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 订阅者资料
/// </summary>
/// <remarks>
/// https://developers.weixin.qq.com/doc/offiaccount/User_Management/Get_users_basic_information_UnionID.html
/// </remarks>
public class WeixinUserInfoJson : WeixinErrorJson
{
    /// <summary>
    /// 用户是否订阅该公众号标识。参考值：1 
    /// </summary>
    /// <remarks>值为0时，代表此用户没有关注该公众号，拉取不到其余信息;值为1时，代表用户已关注该公众号，可以拉取其余信息</remarks>
    
    [JsonPropertyName("subscribe")]
    public int Subscribe { get; set; }

    /// <summary>
    /// 用户关注时间，为时间戳。如果用户曾多次关注，则取最后关注时间
    /// </summary>
    [JsonPropertyName("subscribe_time")]
    public long? SubscribeTime { get; set; }

    /// <summary>
    /// 只有在用户将公众号绑定到微信开放平台帐号后，才会出现该字段。参考值：o6_bmasdasdsad6_2sgVt7hMZOPfL
    /// </summary>
    [JsonPropertyName("unionid")]
    public string UnionId { get; set; }

    /// <summary>
    /// 用户的标识，对当前公众号唯一。参考值：o6_bmjrPTlm6_2sgVt7hMZOPfL2M
    /// </summary>
    [JsonPropertyName("openid")]
    public string OpenId { get; set; }

    /// <summary>
    /// 用户的语言，参考值：zh_CN 为简体中文
    /// </summary>
    /// <remarks><seealse cref="WeixinLanguage"/></remarks>
    [JsonPropertyName("language")]
    public string Language { get; set; }

    /// <summary>
    /// 公众号运营者对粉丝的备注，公众号运营者可在微信公众平台用户管理界面对粉丝添加备注
    /// </summary>
    [JsonPropertyName("remark")]
    public string Remark { get; set; }

    /// <summary>
    /// 用户所在的分组ID（兼容旧的用户分组接口）
    /// </summary>
    [JsonPropertyName("groupid")]
    public int? GroupId { get; set; }

    /// <summary>
    /// 用户被打上的标签ID列表
    /// </summary>
    [JsonPropertyName("tagid_list")]
    public List<int> TagIdList { get; set; }

    /// <summary>
    /// 返回用户关注的渠道来源
    /// </summary>
    /// <remarks>ADD_SCENE_SEARCH 公众号搜索，ADD_SCENE_ACCOUNT_MIGRATION 公众号迁移，ADD_SCENE_PROFILE_CARD 名片分享，ADD_SCENE_QR_CODE 扫描二维码，ADD_SCENE_PROFILE_LINK 图文页内名称点击，ADD_SCENE_PROFILE_ITEM 图文页右上角菜单，ADD_SCENE_PAID 支付后关注，ADD_SCENE_WECHAT_ADVERTISEMENT 微信广告，ADD_SCENE_REPRINT 他人转载 ,ADD_SCENE_LIVESTREAM 视频号直播，ADD_SCENE_CHANNELS 视频号 , ADD_SCENE_OTHERS 其他。注意：2020年6月8日起，用户关注来源“微信广告（ADD_SCENE_WECHAT_ADVERTISEMENT）”从“其他（ADD_SCENE_OTHERS）”中拆分给出。</remarks>
    [JsonPropertyName("subscribe_scene")]
    public string SubscribeScene { get; set; }

    /// <summary>
    /// 二维码扫码场景（开发者自定义）。参考值：98765
    /// </summary>
    [JsonPropertyName("qr_scene")]
    public int? QrScene { get; set; }

    /// <summary>
    /// 二维码扫码场景描述（开发者自定义）
    /// </summary>
    [JsonPropertyName("qr_scene_str")]
    public string QrSceneStr { get; set; }

    #region 2021年12月27日之后，不再输出头像、昵称信息。
    /// <summary>
    /// 昵称
    /// </summary>
    /// <remarks>2021年12月27日之后，不再输出头像、昵称信息。</remarks>
    [JsonPropertyName("nickname")]
    public string Nickname { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    /// <remarks>2021年12月27日之后，不再输出头像、昵称信息。</remarks>
    [JsonPropertyName("headimgurl")]
    public string HeadImgUrl { get; set; }

    [JsonPropertyName("sex")]
    public int? Sex { get; set; }

    /// <summary>
    /// The city name.
    /// </summary>
    [JsonPropertyName("city")]
    public string City { get; set; }

    /// <summary>
    /// The province name.
    /// </summary>
    [JsonPropertyName("province")]
    public string Province { get; set; }

    /// <summary>
    /// The country name.
    /// </summary>
    [JsonPropertyName("country")]
    public string Country { get; set; }
    #endregion
}
