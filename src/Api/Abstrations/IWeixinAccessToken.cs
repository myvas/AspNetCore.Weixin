using Microsoft.Extensions.Primitives;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin
{
    public interface IWeixinAccessToken
    {
        Task<string> GetTokenAsync(CancellationToken cancellationToken = default);
        Task<string> GetTokenAsync(bool forceRenew, CancellationToken cancellationToken=default);
        string GetToken(bool forceRenew);
        string GetToken();
    }
}