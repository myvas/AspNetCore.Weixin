using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Myvas.AspNetCore.Weixin
{
    public interface IRequestMessageBase : IMessageBase
    {
        ReceivedMsgType MsgType { get; }
        long MsgId { get; set; }
    }
    public class RequestMessageBase : MessageBase, IRequestMessageBase
    {
        public RequestMessageBase()
        {

        }

        public virtual ReceivedMsgType MsgType
        {
            get { return ReceivedMsgType.text; }
        }

        public long MsgId { get; set; }
    }
}
