using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Myvas.AspNetCore.Weixin;

namespace WeixinSiteSample.Pages
{
    public class AccessTokenModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IWeixinAccessToken _api;

        public AccessTokenModel(IWeixinAccessToken api, ILogger<IndexModel> logger)
        {
            _api = api;
            _logger = logger;
        }

        public string Token { get; set; }

        public async Task OnGet()
        {
            Token = await _api.GetTokenAsync();
        }
    }
}
