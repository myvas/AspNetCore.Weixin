using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin
{
    public interface IWeixinAccessToken
    {
        string GetToken(bool forceRenew);
        string GetToken();
        Task<string> GetTokenAsync(bool forceRenew);
        Task<string> GetTokenAsync();
    }
}