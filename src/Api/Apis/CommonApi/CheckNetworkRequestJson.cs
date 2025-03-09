using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin;

public class CheckNetworkRequestJson
{
    public CheckNetworkRequestJson() { }
    
    public CheckNetworkRequestJson(string action, string checkOperator)
    {
        Action = action;
        CheckOperator = checkOperator;
    }

    /// <summary>
    /// 执行的检测动作，<see cref="CheckNetworkActions"/>
    /// </summary>
    [JsonPropertyName("action")]
    public string Action { get; set; }

    /// <summary>
    /// 通信运营商，<see cref="CheckNetworkOperators"/>
    /// </summary>
    [JsonPropertyName("check_operator")]
    public string CheckOperator { get; set; }
}
