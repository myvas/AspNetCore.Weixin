using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Myvas.AspNetCore.Weixin;
using WeixinSiteSample.Models;

namespace WeixinSiteSample.Controllers;

public class WeixinController : Controller
{
    private readonly ILogger<WeixinController> _logger;
    private readonly IWeixinUserApi _api;
    private readonly IWeixinCustomerSupportApi _csApi;
    private readonly IWeixinSubscriberStore _subscriberStore;
    private readonly IWeixinResponseMessageStore<WeixinResponseMessageEntity> _responseStore;
    private readonly IWeixinReceivedMessageStore<WeixinReceivedMessageEntity> _messageStore;
    private readonly IWeixinReceivedEventStore<WeixinReceivedEventEntity> _eventStore;

    public WeixinController(
        ILogger<WeixinController> logger,
        IWeixinUserApi api,
        IWeixinCustomerSupportApi csApi,
        IWeixinSubscriberStore subscriberStore,
        IWeixinResponseMessageStore<WeixinResponseMessageEntity> responseStore,
        IWeixinReceivedMessageStore<WeixinReceivedMessageEntity> messageStore,
        IWeixinReceivedEventStore<WeixinReceivedEventEntity> eventStore)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _api = api;
        _csApi = csApi;
        _subscriberStore = subscriberStore ?? throw new ArgumentNullException(nameof(subscriberStore));
        _responseStore=responseStore ?? throw new ArgumentNullException(nameof(responseStore));
        _messageStore = messageStore ?? throw new ArgumentNullException(nameof(messageStore));
        _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
    }

    public async Task<IActionResult> Index()
    {
        var subscriberCount = await _subscriberStore.Items.CountAsync();//.GetSubscribersCountAsync();
        var receivedTextCount = await _messageStore.Items.CountAsync();//.GetAllByReceivedTimeAsync(null, null);
        var vm = new WeixinIndexViewModel() { SubscriberCount = subscriberCount, ReceivedTextCount = receivedTextCount };

        return View(vm);
    }

    public async Task<IActionResult> Subscribers(int? n)
    {
        const int pageSize = 50;

        var totalRecords = await _subscriberStore.GetCountAsync();
        var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

        if (n == null) n = 1;
        else if (n.Value < 1) n = 1;
        else if (n > totalPages) n = totalPages;

        var vm = new ReturnableViewModel<IList<WeixinSubscriberEntity>>();

        var pageIndex = n.Value - 1;
        var subscribers = await _subscriberStore.GetItemsAsync(pageSize, pageIndex);
        _logger.LogDebug($"微信订阅者在数据库中共{totalRecords}条记录。");
        vm.Item = subscribers;
        vm.ReturnUrl=Url.Action(nameof(Subscribers), new { n });

        return View(vm);
    }


    public async Task<IActionResult> ReceivedText(int? n)
    {
        const int pageSize = 50;

        var totalRecords = await _messageStore.GetCountAsync();
        var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

        if (n == null) n = 1;
        else if (n.Value < 1) n = 1;
        else if (n > totalPages) n = totalPages;

        var vm = new ReturnableViewModel<IList<WeixinReceivedMessageEntity>>();

        var pageIndex = n.Value - 1;

        vm.Item = await _messageStore.GetItemsAsync(pageSize, pageIndex);
        vm.ReturnUrl = Url.Action(nameof(ReceivedText), new { n });
        _logger.LogDebug($"微信文本消息在数据库中共{totalRecords}条记录。");
        return View(vm);
    }

    public async Task<IActionResult> SendWeixin(string openId)
    {
        if (string.IsNullOrEmpty(openId))
        {
            return View();
        }

        var vm = new SendWeixinViewModel
        {
            Responsed = await _responseStore.Items.Where(x => x.ToUserName == openId).ToListAsync(),
            OpenId = openId
        };
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

        var result = await _csApi.SendText(vm.OpenId, vm.Content);
        if (!result.Succeeded)
        {
            ModelState.AddModelError("", result.ErrorMessage);
            return View(vm);
        }

        return RedirectToAction(nameof(Index));
    }

    public IActionResult About()
    {
        return View();
    }

    public IActionResult Error()
    {
        return View();
    }
}