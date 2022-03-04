using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Myvas.AspNetCore.Weixin.Site.Extensions;

internal static partial class IRequestMessageEventBaseExtensions
{
    /// <summary>
    /// 将RequestMessageEventBase转换成RequestMessageText类型，其中Content = requestMessage.EventKey
    /// </summary>
    /// <param name="requestMessageEvent"></param>
    /// <returns></returns>
    public static RequestMessageText ConvertToRequestMessageText(this IRequestMessageEventBase requestMessageEvent)
    {
        var requestMessage = requestMessageEvent;
        var requestMessageText = new RequestMessageText()
        {
            FromUserName = requestMessage.FromUserName,
            ToUserName = requestMessage.ToUserName,
            CreateTime = requestMessage.CreateTime,
            MsgId = requestMessage.MsgId,
            Content = requestMessage.EventKey
        };
        return requestMessageText;
    }
}
