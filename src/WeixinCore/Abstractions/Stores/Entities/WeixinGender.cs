using System.ComponentModel.DataAnnotations;

namespace Myvas.AspNetCore.Weixin
{
    public enum WeixinGender
    {
        [Display(Name = "男")]
        Male = 1,
        [Display(Name = "女")]
        Female = 2,
        [Display(Name = "未知")]
        Unknown = 0
    }
}