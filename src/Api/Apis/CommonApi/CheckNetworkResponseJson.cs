using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin;

public class CheckNetworkResponseJson : WeixinErrorJson
{
    [JsonPropertyName("dns")]
    public IList<IpOperatorDnsMap> Dns { get; set; }

    [JsonPropertyName("ping")]
    public IList<IpOperatorPingMap> Ping { get; set; }
}

public class IpOperatorDnsMap
{
    public string ip { get; set; }
    public string real_operator { get; set; }
}

public class IpOperatorPingMap
{
    public string ip { get; set; }
    public string from_operator { get; set; }
    public string package_loss { get; set; }
    public string time { get; set; }
}
