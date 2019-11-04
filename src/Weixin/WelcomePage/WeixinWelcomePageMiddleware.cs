using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin
{
	/// <summary>
	/// This middleware provides a default welcome/validation page for new Weixin App.
	/// </summary>
	public class WeixinWelcomePageMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly HttpClient _backchannel;
		private readonly WeixinWelcomePageOptions _options;
		private readonly ILogger _logger;
		private readonly IWeixinMessageEncryptor _encryptor;

		public WeixinWelcomePageMiddleware(
			RequestDelegate next,
			IOptions<WeixinWelcomePageOptions> options,
			ILoggerFactory loggerFactory,
			IWeixinMessageEncryptor encryptor)
		{
			_next = next ?? throw new ArgumentNullException(nameof(next));
			_logger = loggerFactory?.CreateLogger<WeixinWelcomePageMiddleware>() ?? throw new ArgumentNullException(nameof(loggerFactory));
			_options = options?.Value ?? throw new ArgumentNullException(nameof(options));
			_encryptor = encryptor ?? throw new ArgumentNullException(nameof(encryptor));

			//入参检查
			if (string.IsNullOrEmpty(_options.WebsiteToken))
			{
				throw new ArgumentException($"参数 {nameof(_options.WebsiteToken)} 不能为空。");
			}

			if (string.IsNullOrEmpty(_options.Path))
			{
				throw new ArgumentException($"参数 {nameof(_options.Path)} 不能为空。");
			}

			_backchannel = new HttpClient(new HttpClientHandler());
			_backchannel.DefaultRequestHeaders.UserAgent.ParseAdd("AspNetCoreWeixin");
			_backchannel.Timeout = TimeSpan.FromSeconds(60);
			_backchannel.MaxResponseContentBufferSize = 1024 * 1024 * 10; // 10 MB
		}

		//protected WeixinMessageHandler CreateHandler()
		//{
		//    return new WeixinMessageHandler(_backchannel);
		//}

		/// <summary>
		/// Process an individual request.
		/// </summary>
		/// <param name="context">The <see cref="HttpContext"/>.</param>
		/// <returns></returns>
		public async Task Invoke(HttpContext context)
		{
			var welcomePath = _options.Path;

			HttpRequest request = context.Request;
			if (request.Path == welcomePath)
			{
				// Dynamically generated for LOC.
				if (string.Compare(request.Method, "POST", true) != 0)
				{
					await InvokeGet(context);
				}
				else
				{
					await InvokePost(context);
				}
			}
			else
			{
				await _next(context);
			}
		}

		/// <summary>
		/// 指示微信公众号消息接收地址是否可用
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public async Task InvokeGet(HttpContext context)
		{
			// 明文模式：GET http://wx.steamlet.com/wx?signature=c48abd8c533cba0ff7230e9b5b78d8970bef70e8&echostr=3851421201668733949&timestamp=1553423097&nonce=1968330093
			// 兼容模式：GET http://wx.steamlet.com/wx?signature=2b3a50d442885f8eef72f4d10bef472df12d1675&echostr=4780759847666154895&timestamp=1553423758&nonce=2002608571
			// 安全模式：GET http://wx.steamlet.com/wx?signature=153cf3051ee1de57b31cab8c16a1c08e7ebc7a18&echostr=4882545160925183999&timestamp=1553423855&nonce=659627225

			HttpRequest request = context.Request;
			var signature = request.Query["signature"];
			var echostr = request.Query["echostr"];
			var timestamp = request.Query["timestamp"];
			var nonce = request.Query["nonce"];

			var websiteToken = _options.WebsiteToken;

			//context.Response.Clear();
			context.Response.ContentType = "text/plain;charset=utf-8";

			if (SignatureHelper.ValidateSignature(signature, timestamp, nonce, websiteToken)) //【腾讯微信公众号后台程序】发起服务器地址验证
			{
				await context.Response.WriteAsync(echostr); //返回随机字符串则表示验证通过
				return;
			}
			else//【配置管理员】核实AspNetCore.Weixin服务地址
			{
				string result = "如果你在浏览器中看到这句话，说明本网站是微信公众号服务器，可以在微信公众号后台的“开发/基本配置/服务器配置/服务器地址(URL)”字段中填写此URL！";
				await context.Response.WriteAsync(result);
				return;
			}
		}

		/// <summary>
		/// 微信公众号消息接收地址
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public async Task InvokePost(HttpContext context)
		{
			HttpRequest request = context.Request;
			var signature = request.Query["signature"];
			var timestamp = request.Query["timestamp"];
			var nonce = request.Query["nonce"];

			var websiteToken = _options.WebsiteToken;

			//context.Response.Clear();
			context.Response.ContentType = "text/plain;charset=utf-8";
			if (!SignatureHelper.ValidateSignature(signature, timestamp, nonce, websiteToken) && !_options.Debug)
			{
				var result = "这是一个微信程序，请用微信客户端访问!";
				await context.Response.WriteAsync(result);
				return;
			}

			var messageHandler = new WeixinMessageHandler();
			try
			{
				var result = await messageHandler.InitializeAsync(_options, context, _logger, _encryptor);
				//var result = await messageHandler.HandleAsync();
				//if (!result)
				//{
				//    await _next(context);
				//}
			}
			catch (Exception ex)
			{
				_logger.LogError(0, ex, "处理POST报文时发生异常");
			}
			finally
			{
				try { await messageHandler.TeardownAsync(); }
				catch (Exception)
				{
					// Don't mask the original exception, if any
				}
			}
		}
	}
}
