using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Myvas.AspNetCore.Weixin;

namespace WeixinSiteSample.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IWeixinAccessToken _token;

        public IndexModel(IWeixinAccessToken token, ILogger<IndexModel> logger)
        {
            _token = token;
            _logger = logger;
        }

        public string Token { get; set; }

        public void OnGet()
        {
            Token = _token.GetToken();
        }
    }
}