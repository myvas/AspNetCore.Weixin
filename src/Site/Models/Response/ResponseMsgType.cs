namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 回复/发送消息类型
/// </summary>
public enum ResponseMsgType
{
    /// <summary>
    /// 1 回复/发送文本消息
    /// </summary>
    text,
    /// <summary>
    /// 6 回复/发送图文消息
    /// </summary>
    news,
    /// <summary>
    /// 5 回复/发送音乐消息
    /// </summary>
    music,
    /// <summary>
    /// 2 回复/发送图片消息
    /// </summary>
    image,
    /// <summary>
    /// 3 回复/发送语音消息
    /// </summary>
    voice,
    /// <summary>
    /// 4 回复/发送视频消息
    /// </summary>
    video,
    /// <summary>
    /// 将消息转发到客服
    /// </summary>
    transfer_customer_service
}
