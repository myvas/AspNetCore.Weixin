using AspNetCore.Weixin;
using Demo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Demo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWeixinAccessToken _weixinAccessToken;

        public HomeController(
            ILoggerFactory loggerFactory,
            IWeixinAccessToken smsSender)
        {
            _logger = loggerFactory?.CreateLogger<HomeController>() ?? throw new ArgumentNullException(nameof(loggerFactory));
            _weixinAccessToken = smsSender ?? throw new ArgumentNullException(nameof(smsSender));
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendWeixinArticle(SendArticleViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return Ok(false);
            }

            var result = _weixinAccessToken.GetToken();

            return Ok(result);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
    }
}