using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WeixinSiteSample.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly SignInManager<IdentityUser> _signInManager;

    public HomeController(ILogger<HomeController> logger, SignInManager<IdentityUser> signInManager)
    {
        _logger = logger;
        _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
    }

    public IActionResult Index()
    {
        return View();
    }

    //[Authorize]
    public async Task<IActionResult> UserInfo()
    {
        var model = new UserInfoViewModel()
        {
            AccessToken = await HttpContext.GetTokenAsync("access_token"),
            RefreshToken = await HttpContext.GetTokenAsync("refresh_token"),
            ExpiresAt = await HttpContext.GetTokenAsync("expires_at"),
            TokenType = await HttpContext.GetTokenAsync("token_type"),

            //1.Claims for Identity:
            //http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier : 7668444a-9d51-4a01-8a2f-e899812db37b
            //http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name : 15902059380
            //AspNet.Identity.SecurityStamp : 75a48aaa - 0276 - 40f0 - 9167 - 6bd49f0c5327

            //2.Claims for Identity associated with WeixinOpen:
            //http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier : 187235f3-ed8c-47c9-8052-8a41417ebedb
            //http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name : 15902059380
            //AspNet.Identity.SecurityStamp : IWDARHGKWW5KJOBPMY4QMR4GSMEVRI5S
            //http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod : WeixinOpen

            User = HttpContext.User,

            ExternalLoginInfo = await _signInManager.GetExternalLoginInfoAsync()
        };

        return View(model);
    }

    public IActionResult ShowQrcode(string redirectUrl)
    {
        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        var vm = new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        };
        return View(vm);
    }
}
