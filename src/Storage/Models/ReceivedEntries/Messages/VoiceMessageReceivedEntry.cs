using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin.Models;

/// <summary>
/// 收到语音消息
/// </summary>
[XmlRoot("xml", Namespace = "")]
public class VoiceMessageReceivedEntry : MessageReceivedEntry
{
    /// <summary>
    /// 消息id，64位整型
    /// </summary>
    public string MediaId { get; set; }

    /// <summary>
    /// 语音格式，如amr，speex等
    /// </summary>
    public string Format { get; set; }

    /// <summary>
    /// [Optional] 语音识别结果，UTF8编码
    /// </summary>
    /// <remarks>开通语音识别后，用户每次发送语音给公众号时，微信会在推送的语音消息XML数据包中，增加一个Recongnition字段（注：由于客户端缓存，开发者开启或者关闭语音识别功能，对新关注者立刻生效，对已关注用户需要24小时生效。开发者可以重新关注此帐号进行测试）</remarks>
    public string Recognition { get; set; }
}
