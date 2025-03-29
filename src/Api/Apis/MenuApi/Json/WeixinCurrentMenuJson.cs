using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// The Current Weixin Menu published via API or Weixin Offical Account Management Platform(MP)
/// </summary>
public class WeixinCurrentMenuJson : WeixinErrorJson
{
    [JsonPropertyName("is_menu_open")]
    public int? IsMenuOpen { get; set; }

    [JsonPropertyName("selfmenu_info")]
    public SelfMenuInfoClass SelfMenuInfo { get; set; } = new();

    public class SelfMenuInfoClass
    {
        [JsonPropertyName("button")]
        public List<WeixinMenuJson.Button> Buttons { get; set; } = [];
    }
}



