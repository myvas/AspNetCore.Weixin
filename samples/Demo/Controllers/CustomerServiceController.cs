using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AspNetCore.Weixin;
using Demo.Models;

namespace Demo.Controllers
{
    public class CustomerServiceController : Controller
    {
        private readonly ILogger<CustomerServiceController> _logger;
        private readonly IWeixinAccessToken _weixinAccessToken;

        public CustomerServiceController(ILoggerFactory loggerFactory,
            IWeixinAccessToken weixinAccessToken)
        {
            _logger = loggerFactory?.CreateLogger<CustomerServiceController>() ?? throw new ArgumentNullException(nameof(loggerFactory));
            _weixinAccessToken = weixinAccessToken ?? throw new ArgumentNullException(nameof(weixinAccessToken));
        }

        public IActionResult Index()
        {
            var token = _weixinAccessToken.GetToken();
            ViewData["Token"] = token;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendText(SendTextViewModel vm)
        {
            var token = _weixinAccessToken.GetToken();
            await Custom.SendText(token, vm.OpenId, vm.Content);
            return Ok();
        }
    }
}