using System.Collections.Generic;

namespace Myvas.AspNetCore.Weixin;

public class WeixinGetWhiteListResultJson : WeixinWifiErrorJson
{
    public List<string> ipList = new List<string>();
    public List<string> domainList = new List<string>();
}
