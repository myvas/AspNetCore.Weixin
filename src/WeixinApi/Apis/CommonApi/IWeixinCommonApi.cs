using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin
{
    public interface IWeixinCommonApi
    {
        Task<CheckNetworkResponseJson> CheckNetworkAsync(string action, string checkOperator, CancellationToken cancellationToken = default);
        Task<CheckNetworkResponseJson> CheckNetworkAsync(CheckNetworkRequestJson data, CancellationToken cancellationToken = default);
        Task<IpResponseJson> GetCallbackIpsAsync(CancellationToken cancellationToken = default);
        Task<IpResponseJson> GetTencentServerIpsAsync(CancellationToken cancellationToken = default);
    }
}