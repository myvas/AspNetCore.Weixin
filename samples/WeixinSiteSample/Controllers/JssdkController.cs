using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Myvas.AspNetCore.Weixin;

namespace WeixinSiteSample.Controllers;

public class JssdkController : Controller
{
    private readonly IWeixinJsapiTicketApi _api;
    private readonly WeixinOptions _options;

    public JssdkController(
        IWeixinJsapiTicketApi api,
        IOptions<WeixinOptions> optionsAccessor)
    {
        _api = api ?? throw new ArgumentNullException(nameof(api));
        _options = optionsAccessor?.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));
    }

    public async Task<IActionResult> Index()
    {
        var vm = new ShareJweixinViewModel();

        var config = new WeixinJsConfig()
        {
            debug = true,
            appId = _options.AppId
        };
        var jsapiTicket = await _api.GetTicketAsync();
        var refererUrl = Request.GetAbsoluteUri();// Url.AbsoluteContent(Url.Action());
        vm.ConfigJson = config.ToJson(jsapiTicket.Ticket, refererUrl);

        vm.Title = "链接分享测试";
        vm.Url = "http://demo.auth.myvas.com/jssdk";
        vm.Description = "链接分享测试";
        vm.ImgUrl = "http://demo.auth.myvas.com/img/mp-test.jpg";
        return View(vm);
    }
}