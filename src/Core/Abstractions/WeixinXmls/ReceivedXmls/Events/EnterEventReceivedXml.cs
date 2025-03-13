using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 进入微信号。
/// </summary>
[XmlRoot("xml", Namespace = "")]
public class EnterEventReceivedXml : EventReceivedXml
{
}
