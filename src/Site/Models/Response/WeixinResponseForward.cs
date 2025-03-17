using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 将消息转发到客服，暂不支持将消息转发到指定客服!!!
/// </summary>
/// <remarks>ref to: https://mp.weixin.qq.com/wiki?t=resource/res_main&id=mp1458557405 </remarks>
[XmlRoot("xml", Namespace = "")]
public class WeixinResponseForward : WeixinResponse, IWeixinResponseMessage
{
    public WeixinResponseForward()
    {
        MsgType = ResponseMsgType.transfer_customer_service;
    }

    ///// <summary>
    ///// TODO:将消息转发到指定客服
    ///// </summary>
    //[XmlArrayItem("TransInfo")]
    //public List<KfAccountItem> TransInfo { get; set; }
}
