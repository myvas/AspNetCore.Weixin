using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Myvas.AspNetCore.Weixin
{
    public class WeixinRequest : WeixinMessage, IWeixinRequest
    {
        public WeixinRequest()
        {

        }

        public virtual RequestMsgType MsgType
        {
            get { return RequestMsgType.text; }
        }

        public long MsgId { get; set; }
    }
}
