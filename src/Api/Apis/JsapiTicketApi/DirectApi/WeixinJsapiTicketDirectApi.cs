using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public class WeixinJsapiTicketDirectApi : WeixinSecureApiClient, IWeixinJsapiTicketDirectApi
{
    public WeixinJsapiTicketDirectApi(IOptions<WeixinOptions> optionsAccessor, IWeixinAccessTokenApi tokenProvider) : base(optionsAccessor, tokenProvider)
    {
    }

    /// <summary>
    /// 创建菜单/将菜单上传发布到腾讯服务器
    /// </summary>
    /// <returns></returns>
    public async Task<WeixinJsapiTicketJson> GetTicketAsync(CancellationToken cancellationToken = default)
    {
        var pathAndQuery = "/cgi-bin/ticket/getticket?access_token=ACCESS_TOKEN&type=jsapi";
        var url = Options?.BuildWeixinApiUrl(pathAndQuery);

        return await SecureGetFromJsonAsync<WeixinJsapiTicketJson>(url);
    }

    public WeixinJsapiTicketJson GetTicket()
        => Task.Run(async () => await GetTicketAsync()).Result;
}
