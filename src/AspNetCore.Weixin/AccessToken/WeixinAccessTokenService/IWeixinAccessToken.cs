using System.Threading.Tasks;

namespace AspNetCore.Weixin
{
    public interface IWeixinAccessToken
    {
        string GetToken(bool forceRenew);
        string GetToken();
    }
}