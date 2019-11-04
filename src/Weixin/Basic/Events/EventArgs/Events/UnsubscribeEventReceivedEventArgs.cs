using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// 退订事件
    /// </summary>
    [XmlRoot("xml", Namespace = "")]
    public class UnsubscribeEventReceivedEventArgs : EventReceivedEventArgs
    {
    }
}
