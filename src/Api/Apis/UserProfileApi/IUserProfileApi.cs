using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public interface IUserProfileApi
{
    Task<UserInfoResult> GetUserInfo(string openID, CancellationToken cancellationToken = default);
    Task<UploadMediaResult> UploadMediaFile(MediaType type, string fileName, CancellationToken cancellationToken = default);
}
