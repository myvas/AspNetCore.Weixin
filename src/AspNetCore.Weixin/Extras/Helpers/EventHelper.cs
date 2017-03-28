using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace AspNetCore.Weixin
{
    public static class EventHelper
    {
        public static WeixinEvent GetEventType(XDocument doc)
        {
            return GetEventType(doc.Root.Element("Event").Value);
        }

        public static WeixinEvent GetEventType(string str)
        {
            return (WeixinEvent)Enum.Parse(typeof(WeixinEvent), str, true);
        }
    }
}
