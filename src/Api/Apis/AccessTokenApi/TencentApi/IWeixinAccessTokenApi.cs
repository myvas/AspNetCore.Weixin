using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// WARNING! <see cref="IWeixinAccessTokenApi"/> will directly call the Tencent Server to get a new access token each time. 
    /// So we make it 'internal', and force users to use <see cref="IWeixinAccessToken"/> instead.
    /// </summary>
    internal interface IWeixinAccessTokenApi
    {
        Task<WeixinAccessTokenJson> GetTokenAsync(CancellationToken cancellationToken = default);
    }
}