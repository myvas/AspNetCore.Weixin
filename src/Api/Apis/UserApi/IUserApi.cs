using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public interface IUserApi
{
    Task<UserGetJson> Get(string nextOpenId, CancellationToken cancellationToken = default);
    Task<List<string>> GetAllOpenIds(CancellationToken cancellationToken = default);
    Task<List<UserInfoJson>> GetAllUserInfo(CancellationToken cancellationToken = default);
    Task<UserInfoJson> Info(string openId, WeixinLanguage lang = WeixinLanguage.zh_CN, CancellationToken cancellationToken = default);
}
