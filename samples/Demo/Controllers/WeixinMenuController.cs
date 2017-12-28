using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Weixin;
using Demo.Data;
using Demo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Demo.Controllers
{
    public class WeixinMenuController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWeixinAccessToken _weixinAccessToken;
        private readonly ILogger<WeixinMenuController> _logger;

        public WeixinMenuController(AppDbContext context,
            IWeixinAccessToken weixinAccessToken,
            ILogger<WeixinMenuController> logger)
        {
            _context = context;
            _weixinAccessToken = weixinAccessToken;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IActionResult> Index()
        {
            var token = _weixinAccessToken.GetToken();
            var resultJson = await MenuApi.GetMenuAsync(token);

            var vm = new WeixinJsonViewModel
            {
                Token = token,
                Json = JsonConvert.SerializeObject(resultJson, Formatting.Indented)
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
                    var token = _weixinAccessToken.GetToken();
                    var result = await MenuApi.CreateMenuAsync(token, vm.Json);

                    _logger.LogDebug(result.ToString());

                    return View("UpdateMenuResult", result);
                }
            }

            // If we got this far, something failed; redisplay form.
            return RedirectToAction(nameof(Index), new { input = vm.Json });
        }
    }
}