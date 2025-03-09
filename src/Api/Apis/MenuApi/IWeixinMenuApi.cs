using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public interface IWeixinMenuApi
{
    Task<WeixinErrorJson> DeleteMenuAsync(CancellationToken cancellationToken = default);
    Task<WeixinMenu> GetMenuAsync(CancellationToken cancellationToken = default);
    Task<WeixinErrorJson> PublishMenuAsync(string json, CancellationToken cancellationToken = default);
    Task<WeixinErrorJson> PublishMenuAsync(WeixinMenu menu, CancellationToken cancellationToken = default);
}