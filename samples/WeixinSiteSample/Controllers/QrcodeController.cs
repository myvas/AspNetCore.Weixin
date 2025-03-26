using Microsoft.AspNetCore.Mvc;
using Myvas.AspNetCore.Weixin;

namespace WeixinSiteSample.Controllers;

public class QrcodeController : Controller
{
    private readonly ILogger _logger;
    private readonly IWeixinQrcodeApi _api;

    public QrcodeController(ILoggerFactory loggerFactory,
        IWeixinQrcodeApi api)
    {
        _logger = loggerFactory?.CreateLogger<QrcodeController>() ?? throw new ArgumentNullException(nameof(loggerFactory));
        _api = api;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("[controller]/[action]/{scene}")]
    public async Task<IActionResult> UrlWithScene(string scene)
    {
        var createQrcodeResult = await _api.Create("QR_LIMIT_STR_SCENE", scene);
        return Json(createQrcodeResult);
    }

    [HttpGet("[controller]/[action]/{scene}")]
    public async Task<IActionResult> QrcodeWithScene(string scene)
    {
        var createQrcodeResult = await _api.Create("QR_LIMIT_STR_SCENE", scene);

        var url = _api.ShowQrcode(createQrcodeResult.ticket);

        return Content(url);
    }
}