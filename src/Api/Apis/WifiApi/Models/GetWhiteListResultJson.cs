using System.Collections.Generic;

namespace Myvas.AspNetCore.Weixin;

public class GetWhiteListResultJson : WifiErrorJson
{
    public List<string> ipList = new List<string>();
    public List<string> domainList = new List<string>();
}
