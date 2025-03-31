using Myvas.AspNetCore.Weixin;
using System.ComponentModel.DataAnnotations;

namespace WeixinSiteSample.Models;

public class SendWeixinViewModel
{
    public IList<WeixinResponseMessageEntity> Responsed { get; set; }

    [Required]
    public string OpenId { get; set; }

    public string Nickname { get; set; }

    [Required]
    public string Content { get; set; }
}
