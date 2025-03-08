using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public interface IGroupMessageApi
{
    Task<WeixinErrorJson> DeleteSendMessage(string mediaId);
    Task<SendResult> SendGroupMessageByGroupId(string groupId, string mediaId);
    Task<SendResult> SendGroupMessageByOpenId(string mediaId, params string[] openIds);
}
