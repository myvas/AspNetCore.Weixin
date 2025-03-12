# UseCase: IWeixinJsapiTicketApi

## Controller
```csharp
private readonly IWeixinJsapiTicketApi _api;
private readonly WeixinJssdkOptions _options;

public IActionResult Index()
{
	var weixinJsConfig = new WeixinJsConfig()
	{
		debug = false,
		appId = _options.AppId
	};

	var jsapiTicket = _api.GetTicket();
	var refererUrl = Request.GetAbsoluteUri();
	var vm = new ShareJweixinViewModel
	{
		WeixinJsConfigJson = weixinJsConfig.ToJson(jsapiTicket, refererUrl),
		Title = "xxx",
		Url = Url.AbsoluteAction(...),
		Description = "yyy",
		ImgUrl = "https://...."
	};

	return View(vm);
}
```

## _Layout.cshtml

```csharp
<script src="//res.wx.qq.com/open/js/jweixin-1.4.0.js" asp-append-version="true"></script>
```

## Index.cshtml

```
@model ShareJweixinViewModel

//...

<script type="text/javascript">
$(document).ready(function () {
	wx.config(@Html.Raw(Model.WeixinJsConfigJson);
	wx.ready(function(){
		wx.onMenuShareAppMessage({
			title: '@Html.Raw(Model.Title)', // 分享标题
			desc: '@Html.Raw(Model.Description)', // 分享描述
			link: '@Html.Raw(Model.Url)', // 分享链接
			imgUrl: '@Html.Raw(Model.ImgUrl)', // 分享图标
			type: '', // 分享类型,music、video或link，不填默认为link
			dataUrl: '', // 如果type是music或video，则要提供数据链接，默认为空
			success: function () {
					// 用户确认分享后执行的回调函数
					toastSuccess("分享成功");
			},
			cancel: function () {
					// 用户取消分享后执行的回调函数
					toastWarning("已取消分享");
			}
		});
	});
</script>
```