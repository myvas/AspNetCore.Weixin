using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace AspNetCore.Weixin
{
    public static class EventHelper
    {
        public static EventType GetEventType(XDocument doc)
        {
            return GetEventType(doc.Root.Element("Event").Value);
        }

        public static EventType GetEventType(string str)
        {
            return (EventType)Enum.Parse(typeof(EventType), str, true);
        }
    }
}
