using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public class WeixinMenuApi : WeixinSecureApiClient, IWeixinMenuApi
{
    public WeixinMenuApi(IOptions<WeixinOptions> optionsAccessor, IWeixinAccessTokenApi tokenProvider) : base(optionsAccessor, tokenProvider)
    {
    }

    /// <summary>
    /// 创建菜单/将菜单上传发布到腾讯服务器
    /// </summary>
    /// <returns>
    // </returns>
    /// <remarks>
    /// <code>
    /// var menu = new WeixinMenuCreateJson()
    ///     .AddButton(new WeixinMenuJson.Button.Click{
    ///         //...
    ///     })
    ///     .AddButton(new WeixinMenuJson.Button.Container("container_1")
    ///         .AddButton(...)
    ///         .AddButton(...);
    /// </code>
    /// Possible response:
    /// <code>
    /// {"errcode":0,"errmsg":"ok"}
    /// </code>
    /// <code>
    /// {"errcode":40018,"errmsg":"invalid button name size"}
    /// </code>
    /// </remarks>
    public async Task<WeixinErrorJson> PublishMenuAsync(WeixinMenuCreateJson createJson, CancellationToken cancellationToken = default)
    {
        var pathAndQuery = "/cgi-bin/menu/create?access_token=ACCESS_TOKEN";
        var url = Options?.BuildWeixinApiUrl(pathAndQuery);
        return await SecurePostAsJsonAsync<WeixinMenuCreateJson, WeixinErrorJson>(url, createJson, null, cancellationToken);
    }

    /// <summary>
    /// Publish/create a conditional menu to the Management Platform (MP).
    /// </summary>
    /// <param name="createJson">The Json object by which describes a conditional menu.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <remarks>
    /// <code>
    /// var menu = new WeixinConditionalMenuCreateJson()
    ///     .AddButton(new WeixinMenuJson.Button.Click{
    ///         //...
    ///     })
    ///     .AddButton(new WeixinMenuJson.Button.Container("container_1")
    ///         .AddButton(...)
    ///         .AddButton(...);
    /// </code>
    /// </remarks>
    public async Task<WeixinMenuPublishConditionalMenuResultJson> PublishConditionalMenuAsync(WeixinConditionalMenuCreateJson createJson, CancellationToken cancellationToken = default)
    {
        var pathAndQuery = "/cgi-bin/menu/addconditional?access_token=ACCESS_TOKEN";
        var url = Options?.BuildWeixinApiUrl(pathAndQuery);
        return await SecurePostAsJsonAsync<WeixinConditionalMenuCreateJson, WeixinMenuPublishConditionalMenuResultJson>(url, createJson, null, cancellationToken);
    }

    /// <summary>
    /// 获取公众号当前使用的自定义菜单。
    /// </summary>
    /// <param name="version">
    /// <list type="bullet">
    /// <item>0: The menu is known that created via API calling</item>
    /// <item>1: The manu is known that created and managed via <see href="https://mp.weixin.qq.com">MP (Weixin Official Account Management Platform)</see>.</item>
    /// </list>
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <remarks>
    /// <list type="bullet">
    /// <item>如果公众号是通过API调用设置的菜单，则返回菜单的开发配置。</item>
    /// <item>如果公众号是在公众平台官网通过网站功能发布菜单，则本接口返回运营者设置的菜单配置。</item>
    /// </list>
    /// 请注意：
    /// <list type="number">
    /// <item>第三方平台开发者可以通过本接口，在旗下公众号将业务授权给你后，立即通过本接口检测公众号的自定义菜单配置，
    /// 并通过接口再次给公众号设置好自动回复规则，以提升公众号运营者的业务体验。</item>
    /// <item>本接口与自定义菜单查询接口的不同之处在于，自定义菜单查询接口仅能查询到使用API设置的菜单；
    /// 本接口除了能查看使用API设置的菜单，还可以查看账号通过公众平台官网（mp.weixin.qq.com）中设置的菜单。</item>
    /// <item>认证/未认证的服务号/订阅号，以及接口测试号，均拥有该接口权限。</item>
    /// <item>
    /// 本接口中返回的图片/语音/视频为临时素材（临时素材每次获取都不同，3天内有效，通过素材管理-获取临时素材接口来获取这些素材）；
    /// 本接口返回的图文消息为永久素材素材（通过素材管理-获取永久素材接口来获取这些素材）。</item>
    /// </list>
    /// </remarks>
    /// <seealso cref="GetMenuAsync">Get menu</seealso>
    /// <seealso href="https://developers.weixin.qq.com/doc/offiaccount/Custom_Menus/Querying_Custom_Menus.html">Tencent official document for Weixin official account Menu</seealso>
    public async Task<WeixinCurrentMenuJson> GetCurrentMenuAsync(string version = "", CancellationToken cancellationToken = default)
    {
        var pathAndQuery = "/cgi-bin/get_current_selfmenu_info?access_token=ACCESS_TOKEN";
        if (!string.IsNullOrEmpty(version))
        {
            pathAndQuery += $"&v={version}";
        }
        var url = Options?.BuildWeixinApiUrl(pathAndQuery);
        return await SecureGetFromJsonAsync<WeixinCurrentMenuJson>(url);
    }

    /// <summary>
    /// Get the menu with conditional menu (if applicable) on the Management Platform (MP).
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>The menu and conditional menu (if applicable).</returns>
    /// <seealso cref="GetCurrentMenuAsync">Get current menu</seealso>
    public async Task<WeixinConditionalMenuJson> GetMenuAsync(CancellationToken cancellationToken = default)
    {
        var pathAndQuery = "/cgi-bin/menu/get?access_token=ACCESS_TOKEN";
        var url = Options?.BuildWeixinApiUrl(pathAndQuery);
        return await SecureGetFromJsonAsync<WeixinConditionalMenuJson>(url, cancellationToken);
    }

    /// <summary>
    /// Test the menu with the specified user, to check the menu whether it is as expected or not.
    /// </summary>
    /// <param name="userId">The OpenId or Weixin account of user.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The menu matching with the specified user</returns>
    public async Task<WeixinConditionalMenuJson> TryMatchMenuAsync(string userId, CancellationToken cancellationToken = default)
    {
        var pathAndQuery = "/cgi-bin/menu/trymatch?access_token=ACCESS_TOKEN";
        var url = Options?.BuildWeixinApiUrl(pathAndQuery);
        var data = new
        {
            user_id = userId
        };
        var json = JsonSerializer.Serialize(data);
        return await SecurePostContentAsJsonAsync<WeixinConditionalMenuJson>(url, new StringContent(json), cancellationToken);
    }

    /// <summary>
    /// Delete the conditional menu specified by a <paramref name="MenuId"/>.
    /// </summary>
    /// <param name="menuId">Id of the menu to be deleted. e.g. "208379533"</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<WeixinErrorJson> DeleteConditionalMenuAsync(string menuId, CancellationToken cancellationToken = default)
    {
        var pathAndQuery = "/cgi-bin/menu/delconditional?access_token=ACCESS_TOKEN";
        var url = Options?.BuildWeixinApiUrl(pathAndQuery);
        var data = new
        {
            menuid = menuId
        };
        var json = JsonSerializer.Serialize(data);
        return await SecurePostContentAsJsonAsync<WeixinErrorJson>(url, new StringContent(json), cancellationToken);
    }

    /// <summary>
    /// Delete the menu on the Management Platform (MP).
    /// </summary>
    public async Task<WeixinErrorJson> DeleteMenuAsync(CancellationToken cancellationToken = default)
    {
        var pathAndQuery = "/cgi-bin/menu/delete?access_token=ACCESS_TOKEN";
        var url = Options?.BuildWeixinApiUrl(pathAndQuery);
        return await SecureGetFromJsonAsync<WeixinErrorJson>(url, cancellationToken);
    }
}
