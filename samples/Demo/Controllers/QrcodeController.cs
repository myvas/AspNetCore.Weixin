using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AspNetCore.Weixin;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Net;

namespace Demo.Controllers
{
    public class QrcodeController : Controller
    {
        private readonly ILogger<QrcodeController> _logger;
        private readonly IWeixinAccessToken _weixinAccessToken;

        public QrcodeController(ILoggerFactory loggerFactory,
            IWeixinAccessToken weixinAccessToken)
        {
            _logger = loggerFactory?.CreateLogger<QrcodeController>() ?? throw new ArgumentNullException(nameof(loggerFactory));
            _weixinAccessToken = weixinAccessToken ?? throw new ArgumentNullException(nameof(weixinAccessToken));
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("[controller]/[action]/{scene}")]
        public async Task<IActionResult> UrlWithScene(string scene)
        {
            var accessToken = _weixinAccessToken.GetToken();
            var createQrcodeResult = await QrCode.Create(accessToken, "QR_LIMIT_STR_SCENE", scene);
            return Json(createQrcodeResult);
        }

        [HttpGet("[controller]/[action]/{scene}")]
        public async Task<IActionResult> QrcodeWithScene(string scene)
        {
            var accessToken = _weixinAccessToken.GetToken();
            var createQrcodeResult = await QrCode.Create(accessToken, "QR_LIMIT_STR_SCENE", scene);

            var url = QrCode.ShowQrcode(createQrcodeResult.ticket);

            return Content(url);
        }
    }
}