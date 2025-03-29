using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public interface IWeixinGroupMessageApi
{
    Task<WeixinErrorJson> DeleteSendMessage(string mediaId);
    Task<WeixinSendResult> SendGroupMessageByGroupId(string groupId, string mediaId);
    Task<WeixinSendResult> SendGroupMessageByOpenId(string mediaId, params string[] openIds);
}
