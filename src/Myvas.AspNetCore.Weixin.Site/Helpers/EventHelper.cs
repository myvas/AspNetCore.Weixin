using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Myvas.AspNetCore.Weixin
{
    public static class EventHelper
    {
        public static ReceivedEventType GetEventType(XDocument doc)
        {
            return GetEventType(doc.Root.Element("Event").Value);
        }

        public static ReceivedEventType GetEventType(string str)
        {
            return (ReceivedEventType)Enum.Parse(typeof(ReceivedEventType), str, true);
        }
    }
}
