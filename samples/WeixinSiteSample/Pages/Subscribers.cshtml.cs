using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Myvas.AspNetCore.Weixin;
using Myvas.AspNetCore.Weixin.Models;

namespace WeixinSiteSample.Pages
{
    public class SubscribersModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IWeixinSubscriberManager _api;

        public SubscribersModel(IWeixinSubscriberManager api, ILogger<IndexModel> logger)
        {
            _api = api;
            _logger = logger;
        }

        public List<WeixinSubscriber> Subscribers { get; set; }

        public async Task OnGet()
        {
            Subscribers = await _api.GetAllAsync(true);
        }
    }
}
