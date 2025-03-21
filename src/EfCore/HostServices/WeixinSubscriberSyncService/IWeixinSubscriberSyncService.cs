using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin.EfCore;

public interface IWeixinSubscriberSyncService
{
    Task PullSubscribersAsync(CancellationToken cancellationToken = default);
}
