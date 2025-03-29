using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// Create a menu
/// </summary>
public class WeixinMenuCreateJson
{
    [JsonPropertyName("button")]
    public List<WeixinMenuJson.Button> Buttons { get; set; } = [];

}