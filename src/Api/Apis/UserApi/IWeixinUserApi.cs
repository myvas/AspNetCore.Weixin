using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public interface IWeixinUserApi
{
    Task<WeixinUserGetJson> Get(string nextOpenId, CancellationToken cancellationToken = default);
    Task<List<string>> GetAllOpenIds(CancellationToken cancellationToken = default);
    Task<List<WeixinUserInfoJson>> GetAllUserInfo(CancellationToken cancellationToken = default);
    Task<WeixinUserInfoJson> Info(string openId, WeixinLanguage lang = WeixinLanguage.zh_CN, CancellationToken cancellationToken = default);
}
