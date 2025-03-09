using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin;

public class IpResponseJson : WeixinErrorJson
{
    [JsonPropertyName("ip_list")]
    public IList<string> Ips { get; set; }
}
