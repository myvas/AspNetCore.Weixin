using System.Text.Json;
using Newtonsoft.Json;

namespace Myvas.AspNetCore.Weixin
{
    public class CheckNetworkRequestJson
    {
        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("check_operator")]
        public string CheckOperator { get; set; }
    }
}
