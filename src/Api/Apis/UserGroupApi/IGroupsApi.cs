using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public interface IGroupsApi
{
    Task<CreateGroupResult> Create(string name, CancellationToken cancellationToken = default);
    Task<GroupsJson> Get();
    Task<GetGroupIdResult> GetId(string accessToken, string openId, CancellationToken cancellationToken = default);
    Task<WeixinErrorJson> MemberUpdate(string openId, int toGroupId, CancellationToken cancellationToken = default);
    Task<WeixinErrorJson> Update(int id, string name, CancellationToken cancellationToken = default);
}
