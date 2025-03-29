using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Myvas.AspNetCore.Weixin;

namespace WeixinSiteSample.Controllers;

public class MessagesController : Controller
{
    private readonly ILogger<MessagesController> _logger;
    private readonly IWeixinReceivedMessageStore _store;

    public MessagesController(IWeixinReceivedMessageStore store, ILogger<MessagesController> logger)
    {
        _store = store ?? throw new ArgumentNullException(nameof(store));
        _logger = logger;
    }

    /// <summary>
    /// List subscribers
    /// </summary>
    /// <param name="n">the page number</param>
    /// <returns></returns>
    public async Task<IActionResult> Index(int? n)
    {
        const int pageSize = 50;
        var pageIndex = n ?? 0;

        // Fetch total records and calculate total pages
        var totalRecords = await _store.GetCountAsync();
        var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

        // Validate pageIndex to ensure it's within valid range
        if (pageIndex < 0)
        {
            pageIndex = 0;
        }
        else if (pageIndex >= totalPages)
        {
            pageIndex = totalPages - 1;
        }

        // Fetch items for the current page
        var items = await _store.GetItemsAsync(pageSize, pageIndex) ?? [];

        // Logging for debugging
        Trace.WriteLine($"Messages total: {totalRecords}. Query n: {n}, PageIndex: {pageIndex}");

        // Create the model
        var model = new EntitiesViewModel<WeixinReceivedMessageEntity>
        {
            Items = items,
            PageSize = pageSize,
            TotalRecords = totalRecords,
            TotalPages = totalPages,
            PageIndex = pageIndex
        };

        return View(model);
    }

    public async Task<IActionResult> Details(string id)
    {
        if (string.IsNullOrEmpty(id)) { return NotFound(); }

        var item = await _store.FindByIdAsync(id);
        if (item == null)
        {
            return NotFound();
        }

        return View(item);
    }
}
