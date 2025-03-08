using System.Collections.Generic;
using System.Text.Json;
using Newtonsoft.Json;

namespace Myvas.AspNetCore.Weixin;

public class CheckNetworkResponseJson : WeixinErrorJson
{
    [JsonProperty("dns")]
    public IList<IpOperatorDnsMap> Dns { get; set; }

    [JsonProperty("ping")]
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
