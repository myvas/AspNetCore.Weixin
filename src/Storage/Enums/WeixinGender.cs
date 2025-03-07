using System.ComponentModel.DataAnnotations;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 性别（由微信公众号SDK定义）
/// </summary>
public enum WeixinGender
{
    /// <summary>
    /// 男
    /// </summary>
    [Display(Name = "男")]
    Male = 1,

    /// <summary>
    /// 女
    /// </summary>
    [Display(Name = "女")]
    Female = 2,

    /// <summary>
    /// 未知或其他
    /// </summary>
    [Display(Name = "未知")]
    Unknown = 0
}
