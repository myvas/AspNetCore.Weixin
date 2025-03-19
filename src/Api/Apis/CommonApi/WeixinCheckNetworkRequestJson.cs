using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin;

public class WeixinCheckNetworkRequestJson
{
    public WeixinCheckNetworkRequestJson() { }
    
    public WeixinCheckNetworkRequestJson(string action, string checkOperator)
    {
        Action = action;
        CheckOperator = checkOperator;
    }

    /// <summary>
    /// 执行的检测动作，<see cref="WeixinCheckNetworkActions"/>
    /// </summary>
    [JsonPropertyName("action")]
    public string Action { get; set; }

    /// <summary>
    /// 通信运营商，<see cref="WeixinCheckNetworkOperators"/>
    /// </summary>
    [JsonPropertyName("check_operator")]
    public string CheckOperator { get; set; }
}
