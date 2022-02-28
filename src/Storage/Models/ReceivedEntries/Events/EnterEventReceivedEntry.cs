using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin.Models;

/// <summary>
/// 进入微信公众号（使用微信程序进入公众号，Tencent现已弃用之）
/// </summary>
[XmlRoot("xml", Namespace = "")]
public class EnterEventReceivedEntry : EventReceivedEntry
{
}
