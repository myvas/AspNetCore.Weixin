namespace Myvas.AspNetCore.Weixin
{
    public class WeixinRequestImage : WeixinRequest, IWeixinRequest
    {
        public override RequestMsgType MsgType
        {
            get { return RequestMsgType.image; }
        }

        public string MediaId { get; set; }
        public string PicUrl { get; set; }
    }


}
