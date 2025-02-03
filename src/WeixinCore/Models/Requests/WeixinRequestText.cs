namespace Myvas.AspNetCore.Weixin
{
    public class WeixinRequestText : WeixinRequest, IWeixinRequest
    {
        public override RequestMsgType MsgType
        {
            get { return RequestMsgType.text; }
        }
        public string Content { get; set; }
    }


}
