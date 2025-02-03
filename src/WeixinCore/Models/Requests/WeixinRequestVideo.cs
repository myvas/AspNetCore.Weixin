using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Myvas.AspNetCore.Weixin
{


    public class WeixinRequestVideo : WeixinRequest, IWeixinRequest
    {
        public override RequestMsgType MsgType
        {
            get { return RequestMsgType.video; }
        }

        public string MediaId { get; set; }
        public string ThumbMediaId { get; set; }
    }


}
