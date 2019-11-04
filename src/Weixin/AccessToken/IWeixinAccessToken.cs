using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin
{
    public interface IWeixinAccessToken
    {
        string GetToken(bool forceRenew);
        string GetToken();
    }
}