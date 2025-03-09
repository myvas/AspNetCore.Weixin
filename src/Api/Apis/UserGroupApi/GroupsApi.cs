using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 用户组接口
/// </summary>
public class GroupsApi : SecureWeixinApiClient, IGroupsApi
{
    public GroupsApi(IOptions<WeixinOptions> optionsAccessor, IWeixinAccessTokenApi tokenProvider) : base(optionsAccessor, tokenProvider)
    {
    }

    /// <summary>
    /// 创建分组
    /// </summary>
    /// <returns></returns>
    public async Task<CreateGroupResult> Create(string name, CancellationToken cancellationToken = default)
    {
        var pathAndQuery = "/cgi-bin/groups/create?access_token={0}";
        var url = Options?.BuildWeixinApiUrl(pathAndQuery);
        url = await FormatUrlWithTokenAsync(url, cancellationToken);

        var data = new
        {
            group = new
            {
                name = name
            }
        };
        return await PostAsJsonAsync<object, CreateGroupResult>(url, data);
    }

    /// <summary>
    /// 发送文本信息
    /// </summary>
    /// <param name="accessToken"></param>
    /// <returns></returns>
    public async Task<GroupsJson> Get()
    {
        var pathAndQuery = "/cgi-bin/groups/get?access_token={0}";
        var url = Options?.BuildWeixinApiUrl(pathAndQuery);

        return await SecureGetFromJsonAsync<GroupsJson>(url);
    }

    /// <summary>
    /// 获取用户分组
    /// </summary>
    /// <param name="accessToken"></param>
    /// <param name="openId"></param>
    /// <returns></returns>
    public async Task<GetGroupIdResult> GetId(string accessToken, string openId, CancellationToken cancellationToken = default)
    {
        var pathAndQuery = "/cgi-bin/groups/getid?access_token={0}";
        var url = Options?.BuildWeixinApiUrl(pathAndQuery);
        url = await FormatUrlWithTokenAsync(url, cancellationToken);

        var data = new { openid = openId };
        return await PostAsJsonAsync<object, GetGroupIdResult>(url, data);
    }

    /// <summary>
    /// 创建分组
    /// </summary>
    /// <param name="accessToken"></param>
    /// <param name="id"></param>
    /// <param name="name">分组名字（30个字符以内）</param>
    /// <returns></returns>
    public async Task<WeixinErrorJson> Update(int id, string name, CancellationToken cancellationToken = default)
    {
        var pathAndQuery = "/cgi-bin/groups/update?access_token={0}";
        var url = Options?.BuildWeixinApiUrl(pathAndQuery);
        url = await FormatUrlWithTokenAsync(url, cancellationToken);

        var data = new
        {
            group = new
            {
                id = id,
                name = name
            }
        };
        return await PostAsJsonAsync<object, WeixinErrorJson>(url, data);
    }

    /// <summary>
    /// 移动用户分组
    /// </summary>
    /// <param name="accessToken"></param>
    /// <param name="openId"></param>
    /// <param name="toGroupId"></param>
    /// <returns></returns>
    public async Task<WeixinErrorJson> MemberUpdate(string openId, int toGroupId, CancellationToken cancellationToken = default)
    {
        var pathAndQuery = "/cgi-bin/groups/members/update?access_token={0}";
        var url = Options?.BuildWeixinApiUrl(pathAndQuery);
        url = await FormatUrlWithTokenAsync(url, cancellationToken);

        var data = new
        {
            openid = openId,
            to_groupid = toGroupId
        };
        return await PostAsJsonAsync<object, WeixinErrorJson>(url, data);
    }
}
