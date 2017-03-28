using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetCore.Weixin
{
    public class GetWhiteListResultJson : WifiErrorJson
    {
        public List<string> ipList = new List<string>();
        public List<string> domainList = new List<string>();
    }
}
