using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin;

#pragma warning disable 1591

/// <summary>
/// 订阅者资料
/// </summary>
/// <remarks>
/// https://developers.weixin.qq.com/doc/offiaccount/User_Management/Get_users_basic_information_UnionID.html
/// </remarks>
public class UserInfoJson : WeixinErrorJson
{
    /// <summary>
    /// 用户是否订阅该公众号标识。参考值：1 
    /// </summary>
    /// <remarks>值为0时，代表此用户没有关注该公众号，拉取不到其余信息;值为1时，代表用户已关注该公众号，可以拉取其余信息</remarks>
    public int subscribe { get; set; }
    /// <summary>
    /// If unsubscribed?
    /// </summary>
    [JsonIgnore]
    public bool Unsubscribed { get { return subscribe != 1; } set { subscribe = value ? 0 : 1; } }

    /// <summary>
    /// 用户关注时间，为时间戳。如果用户曾多次关注，则取最后关注时间
    /// </summary>
    public long? subscribe_time { get; set; }
    [JsonIgnore]
    public DateTime? SubscribeTime
    {
        get
        {
            if (subscribe_time.HasValue)
                return WeixinTimestampHelper.ToUtcTime(subscribe_time!.Value);
            else
                return null;
        }
        set
        {
            if (value == null)
                subscribe_time = null;
            else
                subscribe_time = WeixinTimestampHelper.FromUtcTime(value!.Value);
        }
    }

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
    public int? groupid { get; set; }

    /// <summary>
    /// 用户被打上的标签ID列表
    /// </summary>
    public List<int> tagid_list { get; set; }

    /// <summary>
    /// 返回用户关注的渠道来源
    /// </summary>
    /// <remarks>ADD_SCENE_SEARCH 公众号搜索，ADD_SCENE_ACCOUNT_MIGRATION 公众号迁移，ADD_SCENE_PROFILE_CARD 名片分享，ADD_SCENE_QR_CODE 扫描二维码，ADD_SCENE_PROFILE_LINK 图文页内名称点击，ADD_SCENE_PROFILE_ITEM 图文页右上角菜单，ADD_SCENE_PAID 支付后关注，ADD_SCENE_WECHAT_ADVERTISEMENT 微信广告，ADD_SCENE_REPRINT 他人转载 ,ADD_SCENE_LIVESTREAM 视频号直播，ADD_SCENE_CHANNELS 视频号 , ADD_SCENE_OTHERS 其他。注意：2020年6月8日起，用户关注来源“微信广告（ADD_SCENE_WECHAT_ADVERTISEMENT）”从“其他（ADD_SCENE_OTHERS）”中拆分给出。</remarks>
    public string subscribe_scene { get; set; }

    /// <summary>
    /// 二维码扫码场景（开发者自定义）。参考值：98765
    /// </summary>
    public int? qr_scene { get; set; }

    /// <summary>
    /// 二维码扫码场景描述（开发者自定义）
    /// </summary>
    public string qr_scene_str { get; set; }

    #region 2021年12月27日之后，不再输出头像、昵称信息。
    /// <summary>
    /// 昵称
    /// </summary>
    /// <remarks>2021年12月27日之后，不再输出头像、昵称信息。</remarks>
    [JsonPropertyName("nickname")]
    public string NickName { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    /// <remarks>2021年12月27日之后，不再输出头像、昵称信息。</remarks>
    [JsonPropertyName("headimgurl")]
    public string AvatorImageUrl { get; set; }

    public int? sex { get; set; }
    [JsonIgnore]
    public WeixinGender? Gender
    {
        get
        {
            if (sex.HasValue)
                return sex!.Value == 0 ? WeixinGender.Male : WeixinGender.Female;
            else
                return null;
        }
        set
        {
            if (value.HasValue)
                sex = (int)value!.Value;
            else
                sex = null;
        }
    }

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
