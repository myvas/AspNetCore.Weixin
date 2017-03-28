using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetCore.Weixin
{
    public interface IRequestMessageBase : IMessageBase
    {
        RequestMsgType MsgType { get; }
        long MsgId { get; set; }
    }
    public class RequestMessageBase : MessageBase, IRequestMessageBase
    {
        public RequestMessageBase()
        {

        }

        public virtual RequestMsgType MsgType
        {
            get { return RequestMsgType.Text; }
        }

        public long MsgId { get; set; }
    }
}
