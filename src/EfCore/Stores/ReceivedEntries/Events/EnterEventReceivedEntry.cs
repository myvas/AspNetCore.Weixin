using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin.Models;

/// <summary>
/// 进入公众号事件
/// </summary>
[XmlRoot("xml", Namespace = "")]
public class EnterEventReceivedEntry : EventReceivedEntry
{
}
