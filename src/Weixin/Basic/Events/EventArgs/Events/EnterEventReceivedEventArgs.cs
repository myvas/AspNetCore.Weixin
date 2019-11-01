using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// 进入微信号。
    /// </summary>
    [XmlRoot("xml", Namespace = "")]
    public class EnterEventReceivedEventArgs : EventReceivedEventArgs
    {
    }
}
