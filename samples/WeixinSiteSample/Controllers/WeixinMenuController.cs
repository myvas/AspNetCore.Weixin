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
    private readonly AppDbContext _context;
    private readonly ILogger<WeixinMenuController> _logger;
    private readonly IWeixinMenuApi _api;

    public WeixinMenuController(AppDbContext context,
        IWeixinMenuApi api,
        ILogger<WeixinMenuController> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _api = api ?? throw new ArgumentNullException(nameof(api));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IActionResult> Index()
    {
        var menu = await _api.GetMenuAsync();

        var vm = new WeixinJsonViewModel
        {
            Json = JsonSerializer.Serialize(menu)// JsonConvert.SerializeObject(resultJson, Formatting.Indented)
        };
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateWeixinMenu(WeixinJsonViewModel vm)
    {
        if (ModelState.IsValid)
        {
            if (!string.IsNullOrEmpty(vm.Json))
            {
                var result = await _api.PublishMenuAsync(vm.Json);

                _logger.LogDebug(result.ToString());

                return View("UpdateMenuResult", result);
            }
        }

        // If we got this far, something failed; redisplay form.
        return RedirectToAction(nameof(Index), new { input = vm.Json });
    }
}