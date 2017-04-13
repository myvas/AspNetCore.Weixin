using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Demo.Models;
using AspNetCore.Jweixin;
using Microsoft.Extensions.Options;
using AspNetCore.Weixin;

namespace Demo.Controllers
{
    public class JssdkController : Controller
    {
        private readonly IWeixinAccessToken _weixinAccessToken;
        private readonly WeixinAccessTokenOptions _options;

        public JssdkController(
            IWeixinAccessToken weixinAccessToken, 
            IOptions<WeixinAccessTokenOptions> optionsAccessor)
        {
            _weixinAccessToken = weixinAccessToken ?? throw new ArgumentNullException(nameof(weixinAccessToken));
            _options = optionsAccessor?.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));
        }

        public IActionResult Index()
        {
            var weixinAccessToken = _weixinAccessToken.GetToken();
            var jsapiTicket = JsapiTicket.Default.GetTicket(weixinAccessToken);

            var vm = new JweixinViewModel();
            vm.Config = new WeixinJsConfig()
            {
                debug = true,
                appId = _options.AppId
            };

            var refererUrl = Url.Action();
            ViewData["ConfigJson"] = vm.Config.ToJson(jsapiTicket, refererUrl);

            ViewData["Link"] = "http://ruhu.daqianit.com/immigrationevals/creat/123456789012345678";
            return View();
        }
    }
}