﻿using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 退订事件
/// </summary>
[XmlRoot("xml", Namespace = "")]
public class UnsubscribeEventReceivedXml : EventReceivedXml
{
}
