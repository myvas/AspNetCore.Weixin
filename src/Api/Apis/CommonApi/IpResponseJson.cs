using System.Collections.Generic;
using System.Text.Json;
using Newtonsoft.Json;

namespace Myvas.AspNetCore.Weixin
{
    public class IpResponseJson : WeixinErrorJson
    {
        [JsonProperty("ip_list")]
        public IList<string> Ips { get; set; }
    }
}
