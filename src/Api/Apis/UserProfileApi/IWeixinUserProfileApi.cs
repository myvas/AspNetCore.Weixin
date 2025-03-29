using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public interface IWeixinUserProfileApi
{
    Task<WeixinUserInfoResult> GetUserInfo(string openID, CancellationToken cancellationToken = default);
    Task<WeixinUploadMediaResult> UploadMediaFile(WeixinMediaType type, string fileName, CancellationToken cancellationToken = default);
}
