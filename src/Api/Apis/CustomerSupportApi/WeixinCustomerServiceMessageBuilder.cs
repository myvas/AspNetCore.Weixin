namespace Myvas.AspNetCore.Weixin;

public static class WeixinCustomerServiceMessageBuilder
{
    public static WeixinCustomerServiceMessage Create(string type)
    {
        if (type == null) type = "";
        return type switch
        {
            WeixinCustomerServiceMessageTypes.Text => new WeixinCustomerServiceMessageText(),
            _ => new WeixinCustomerServiceMessage(),
        };
    }
}
