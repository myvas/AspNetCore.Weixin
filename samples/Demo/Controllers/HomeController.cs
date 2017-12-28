using AspNetCore.Weixin;
using Demo.Data;
using Demo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;
        private readonly ILogger<HomeController> _logger;
        private readonly IWeixinAccessToken _weixinAccessToken;

        public HomeController(
            AppDbContext db,
            ILoggerFactory loggerFactory,
            IWeixinAccessToken smsSender)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _logger = loggerFactory?.CreateLogger<HomeController>() ?? throw new ArgumentNullException(nameof(loggerFactory));
            _weixinAccessToken = smsSender ?? throw new ArgumentNullException(nameof(smsSender));
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Subscribers()
        {
            var vm = new ReturnableViewModel<IList<UserInfoJson>>();

            var token = _weixinAccessToken.GetToken();
            var subscribers = await UserApi.GetAllUserInfo(token);
            vm.Item = subscribers;

            return View(vm);
        }


        public async Task<IActionResult> ReceivedText()
        {
            var items = await _db.ReceivedTextMessages.ToListAsync();
            _logger.LogDebug($"微信文本消息在数据库中共{_db.ReceivedTextMessages.ToList().Count()}条记录。");
            return View(items);
        }

        public async Task<IActionResult> SendWeixin(string openId)
        {
            if (string.IsNullOrEmpty(openId))
            {
                return View();
            }

            var vm = new SendWeixinViewModel();
            vm.Received = await _db.ReceivedTextMessages.Where(x => x.To == openId).ToListAsync();
            vm.OpenId = openId;
            return View(vm);
        }

        [HttpPost, ActionName(nameof(SendWeixin))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendWeixin_Post(SendWeixinViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var token = _weixinAccessToken.GetToken();
            var result = await Custom.SendText(token, vm.OpenId, vm.Content);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", result.errmsg);
                return View(vm);
            }

            return RedirectToAction(nameof(Index));
        }



        public IActionResult About()
        {
            return View();
        }
    }
}