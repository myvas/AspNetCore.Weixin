using Myvas.AspNetCore.Weixin.Models;

namespace Myvas.AspNetCore.Weixin
{
    public interface IWeixinSubscriberManager
    {
        Task<List<WeixinSubscriber>> GetAllAsync(bool forceRenew);
    }
}