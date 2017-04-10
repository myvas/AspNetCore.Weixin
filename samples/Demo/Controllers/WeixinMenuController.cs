using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AspNetCore.Weixin;

namespace Demo.Controllers
{
    public class WeixinMenuController : Controller
    {
        private readonly ILogger<WeixinMenuController> _logger;
        private readonly IWeixinAccessToken _weixinAccessToken;

        public WeixinMenuController(ILoggerFactory loggerFactory,
            IWeixinAccessToken weixinAccessToken)
        {
            _logger = loggerFactory?.CreateLogger<WeixinMenuController>() ?? throw new ArgumentNullException(nameof(loggerFactory));
            _weixinAccessToken = weixinAccessToken ?? throw new ArgumentNullException(nameof(weixinAccessToken));
        }

        public IActionResult Index()
        {
            var token = _weixinAccessToken.GetToken();
            ViewData["Token"] = token;
            return View();
        }
    }
}