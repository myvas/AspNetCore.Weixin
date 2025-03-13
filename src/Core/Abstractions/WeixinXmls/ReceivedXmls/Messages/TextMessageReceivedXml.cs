namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// 收到文本消息
    /// </summary>
    public class TextMessageReceivedXml : MessageReceivedXml
    {
        public string Content { get; set; }
    }
}
