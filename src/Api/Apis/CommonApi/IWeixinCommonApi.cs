using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public interface IWeixinCommonApi
{
    Task<WeixinCheckNetworkResponseJson> CheckNetworkAsync(string action, string checkOperator, CancellationToken cancellationToken = default);
    Task<WeixinCheckNetworkResponseJson> CheckNetworkAsync(WeixinCheckNetworkRequestJson data, CancellationToken cancellationToken = default);
    Task<WeixinIpResponseJson> GetCallbackIpsAsync(CancellationToken cancellationToken = default);
    Task<WeixinIpResponseJson> GetTencentServerIpsAsync(CancellationToken cancellationToken = default);
}