using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Myvas.AspNetCore.Weixin;

public interface IWeixinMenuItemHasKey
{
    [JsonProperty("key")]
    [MaxLength(128, ErrorMessage = "菜单KEY值不能超过128个字节")]
    string Key { get; set; }
}
