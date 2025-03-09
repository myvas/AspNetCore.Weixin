using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public class WeixinMenuApi : SecureWeixinApiClient, IWeixinMenuApi
{
    public WeixinMenuApi(IOptions<WeixinOptions> optionsAccessor, IWeixinAccessTokenApi tokenProvider) : base(optionsAccessor, tokenProvider)
    {
    }

    /// <summary>
    /// 创建菜单/将菜单上传发布到腾讯服务器
    /// </summary>
    /// <returns></returns>
    public async Task<WeixinErrorJson> PublishMenuAsync(string json, CancellationToken cancellationToken = default)
    {
        var pathAndQuery = "/cgi-bin/menu/create?access_token=ACCESS_TOKEN";
        var url = Options?.BuildWeixinApiUrl(pathAndQuery);

        return await SecurePostContentAsJsonAsync<WeixinErrorJson>(url, new StringContent(json), cancellationToken);
    }

    public Task<WeixinErrorJson> PublishMenuAsync(WeixinMenu menu, CancellationToken cancellationToken = default)
        => PublishMenuAsync(WeixinMenuJsonSerializerForApi.Serialize(menu), cancellationToken);


    public async Task<WeixinMenu> GetMenuAsync(CancellationToken cancellationToken = default)
    {
        var pathAndQuery = "/cgi-bin/get_current_selfmenu_info?access_token=ACCESS_TOKEN";
        var url = Options?.BuildWeixinApiUrl(pathAndQuery);
        url = await FormatUrlWithTokenAsync(url, cancellationToken);

        //var json = await Http.GetStringAsync(api);
        var json = await Http.GetStreamAsync(url);
        var menu = await WeixinMenuJsonDeserializerForApi.DeserializeAsync(json, cancellationToken);
        return menu;
    }

    /// <summary>
    /// 删除菜单
    /// </summary>
    public async Task<WeixinErrorJson> DeleteMenuAsync(CancellationToken cancellationToken = default)
    {
        var pathAndQuery = "/cgi-bin/menu/delete?access_token=ACCESS_TOKEN";
        var url = Options?.BuildWeixinApiUrl(pathAndQuery);

        return await SecureGetFromJsonAsync<WeixinErrorJson>(url, cancellationToken);
    }
}
