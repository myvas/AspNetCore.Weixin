using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Myvas.AspNetCore.Weixin;
using System.Text.Json;
using WeixinSiteSample.Data;
using WeixinSiteSample.Models;

namespace WeixinSiteSample.Controllers;

[Authorize]//(Policy = "WeixinMenuManager")]
public class WeixinMenuController : Controller
{
    private readonly ILogger<WeixinMenuController> _logger;
    private readonly IWeixinMenuApi _api;

    public WeixinMenuController(IWeixinMenuApi api,
        ILogger<WeixinMenuController> logger)
    {
        _api = api ?? throw new ArgumentNullException(nameof(api));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IActionResult> Index()
    {
        var menu = await _api.GetCurrentMenuAsync();

        var vm = new WeixinJsonViewModel
        {
            Json = JsonSerializer.Serialize(menu)
        };
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateWeixinMenu(WeixinJsonViewModel input)
    {
        if (ModelState.IsValid)
        {
            if (!string.IsNullOrEmpty(input.Json))
            {
                var menu = JsonSerializer.Deserialize<WeixinMenuCreateJson>(input.Json);
                var result = await _api.PublishMenuAsync(menu);

                _logger.LogDebug(result.ToString());

                var vm = new ReturnableViewModel<IWeixinErrorJson>()
                {
                    Item = result,
                    ReturnUrl = Url.Action(nameof(Index))
                };
                return View("UpdateMenuResult", vm);
            }
        }

        // If we got this far, something failed; redisplay form.
        return RedirectToAction(nameof(Index), new { input = input.Json });
    }
}