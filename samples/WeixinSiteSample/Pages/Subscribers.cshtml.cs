using Microsoft.AspNetCore.Mvc.RazorPages;
using Myvas.AspNetCore.Weixin;
using Myvas.AspNetCore.Weixin.Models;

namespace WeixinSiteSample.Pages
{
    public class SubscribersModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly WeixinSubscriberManager<ApplicationUser> _api;

        public SubscribersModel(WeixinSubscriberManager<ApplicationUser> api, ILogger<IndexModel> logger)
        {
            _api = api;
            _logger = logger;
        }

        public List<ApplicationUser> Subscribers { get; set; }

        public async Task OnGet()
        {
            Subscribers = await _api.GetAllAsync(true);
        }
    }
}
