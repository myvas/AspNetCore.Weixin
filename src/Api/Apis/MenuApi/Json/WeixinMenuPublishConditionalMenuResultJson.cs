using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin;

public class WeixinMenuPublishConditionalMenuResultJson : WeixinErrorJson
{
    /// <summary>
    /// The menu Id that created by this request.
    /// </summary>
    [JsonPropertyName("menuid")]
    public string MenuId { get; set; }
}