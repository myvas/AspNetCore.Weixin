using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin;

public class WeixinIpResponseJson : WeixinErrorJson
{
    [JsonPropertyName("ip_list")]
    public IList<string> Ips { get; set; }
}
