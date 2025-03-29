using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// Create a conditional menu
/// </summary>
public class WeixinConditionalMenuCreateJson
{
    [JsonPropertyName("button")]
    public List<WeixinMenuJson.Button> Buttons { get; set; } = [];

    [JsonPropertyName("matchrule")]
    public WeixinConditionalMenuJson.MatchRule MatchRule{get;set;}=new();
}