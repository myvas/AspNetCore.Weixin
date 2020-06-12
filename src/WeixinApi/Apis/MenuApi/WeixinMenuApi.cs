using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin
{
    public class WeixinMenuApi : SecureApiClient, IWeixinMenuApi
    {
        public WeixinMenuApi(IOptions<WeixinApiOptions> optionsAccessor, IWeixinAccessToken tokenProvider) : base(optionsAccessor, tokenProvider)
        {
        }

        /// <summary>
        /// 创建菜单/将菜单上传发布到腾讯服务器
        /// </summary>
        /// <returns></returns>
        public async Task<WeixinErrorJson> PublishMenuAsync(string json, CancellationToken cancellationToken = default)
        {
            var api = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token=ACCESS_TOKEN";
            var accessToken = await _tokenProvider.GetTokenAsync();
            api = api.Replace("ACCESS_TOKEN", accessToken);

            return await PostContentAsJsonAsync<WeixinErrorJson>(api, new StringContent(json), cancellationToken);
        }

        public Task<WeixinErrorJson> PublishMenuAsync(WeixinMenu menu, CancellationToken cancellationToken = default)
            => PublishMenuAsync(new WeixinMenuJsonSerializerForApi().Serialize(menu), cancellationToken);


        public async Task<WeixinMenu> GetMenuAsync(CancellationToken cancellationToken = default)
        {
            var api = "https://api.weixin.qq.com/cgi-bin/get_current_selfmenu_info?access_token=ACCESS_TOKEN";
            var accessToken = await _tokenProvider.GetTokenAsync();
            api = api.Replace("ACCESS_TOKEN", accessToken);

            //var json = await Http.GetStringAsync(api);
            var json = await Http.GetStreamAsync(api);
            var menu = await new WeixinMenuJsonDeserializerForApi().DeserializeAsync(json, cancellationToken);
            return menu;
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        public async Task<WeixinErrorJson> DeleteMenuAsync(CancellationToken cancellationToken = default)
        {
            var api = "https://api.weixin.qq.com/cgi-bin/menu/delete?access_token=ACCESS_TOKEN";
            var accessToken = await _tokenProvider.GetTokenAsync();
            api = api.Replace("ACCESS_TOKEN", accessToken);

            return await GetFromJsonAsync<WeixinErrorJson>(api, cancellationToken);
        }
    }
}
