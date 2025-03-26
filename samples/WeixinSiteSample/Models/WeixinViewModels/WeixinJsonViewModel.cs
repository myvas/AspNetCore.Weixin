using System.ComponentModel.DataAnnotations;

namespace WeixinSiteSample.Models;

public class WeixinJsonViewModel
{
    public string? AppId { get; set; }

    public string? Token { get; set; }

    [MaxLength(102400)]
    public string? Json { get; set; }
}
