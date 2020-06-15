﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin
{

	/// <summary>
	/// 微信请求的集中处理方法
	/// </summary>
	public class WeixinMessageHandler
	{
		public HttpContext HttpContext { get; private set; }
		private WelcomePageOptions _options;
		private ILogger _logger;
		private IWeixinMessageEncryptor _encryptor;

		protected WeixinMessageHandleResult InitializeResult { get; set; }


		internal async Task<bool> InitializeAsync(
			WelcomePageOptions options,
			HttpContext context,
			ILogger logger,
			IWeixinMessageEncryptor encryptor)
		{
			_options = options ?? throw new ArgumentNullException(nameof(options));
			HttpContext = context ?? throw new ArgumentNullException(nameof(context));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_encryptor = encryptor ?? throw new ArgumentNullException(nameof(encryptor));

			context.Response.OnStarting(OnStartingCallback, this);
			{
				InitializeResult = await HandleOnceAsync();
				if (InitializeResult?.Handled == true)
				{
					return true;
				}
				if (InitializeResult?.Failure != null)
				{
					_logger.LogWarning(0, InitializeResult.Failure, InitializeResult.Failure.Message);
				}
			}
			return false;
		}

		#region Callback
		private static async Task OnStartingCallback(object state)
		{
			var handler = (WeixinMessageHandler)state;
			await handler.FinishResponseOnce();
		}

		private bool _finishCalled;
		private async Task FinishResponseOnce()
		{
			if (!_finishCalled)
			{
				_finishCalled = true;
				await FinishResponseAsync();
			}
		}

		protected virtual Task FinishResponseAsync()
		{
			return TaskCache.CompletedTask;
		}
		#endregion
		#region TearDown        
		/// <summary>
		/// Called once after Invoke by the Middleware.
		/// </summary>
		/// <returns>async completion</returns>
		internal async Task TeardownAsync()
		{
			try
			{
				await FinishResponseOnce();
			}
			finally
			{
			}
		}
		#endregion

		private Task<WeixinMessageHandleResult> _handleTask;
		protected Task<WeixinMessageHandleResult> HandleOnceAsync()
		{
			if (_handleTask == null)
			{
				_handleTask = HandleAsync();
			}
			return _handleTask;
		}
		protected async Task<WeixinMessageHandleResult> HandleOnceSafeAsync()
		{
			try
			{
				return await HandleOnceSafeAsync();
			}
			catch (Exception ex)
			{
				return WeixinMessageHandleResult.Fail(ex);
			}
		}

		/// <summary>
		/// 执行微信请求
		/// </summary>
		protected async Task<WeixinMessageHandleResult> HandleAsync()
		{
			bool handled = false;

			var xml = new StreamReader(HttpContext.Request.Body).ReadToEnd();
			_logger.LogDebug("Request Body({0}): {1}", xml?.Length, xml);

			//Try decrypt if needed
			var encryptType = HttpContext.Request.Query["encrypt_type"]; //aes
			_logger.LogDebug("encrypt_type: {0}", encryptType);
			var isEncrypted = encryptType == "aes";
			_logger.LogDebug("isEncrypted: {0}", isEncrypted);
			if (isEncrypted)
			{
				var msg_signature = HttpContext.Request.Query["msg_signature"];
				_logger.LogDebug("msg_signature: {0}", msg_signature);
				var timestamp = HttpContext.Request.Query["timestamp"];
				_logger.LogDebug("timestamp: {0}", timestamp);
				var nonce = HttpContext.Request.Query["nonce"];
				_logger.LogDebug("nonce: {0}", nonce);

				var decryptedXml = _encryptor.Decrypt(msg_signature, timestamp, nonce, xml);
				_logger.LogDebug("Decrypted Request Body({0}): {1}", decryptedXml?.Length, decryptedXml);

				xml = decryptedXml;
			}

			var received = MyvasXmlConvert.DeserializeObject<ReceivedXml>(xml);
			switch (received.MsgType)
			{
				case ReceivedMsgType.@event:
					{
						var ev = MyvasXmlConvert.DeserializeObject<EventReceivedXml>(xml);
						switch (ev.Event)
						{
							case ReceivedEventType.subscribe:
								{
									var x = MyvasXmlConvert.DeserializeObject<SubscribeEventReceivedXml>(xml);
									var ctx = new WeixinReceivedContext<SubscribeEventReceivedXml>(this, x, isEncrypted);
									handled = await _options.Events.SubscribeEventReceived(ctx);
								}
								break;
							case ReceivedEventType.unsubscribe:
								{
									var x = MyvasXmlConvert.DeserializeObject<UnsubscribeEventReceivedXml>(xml);
									var ctx = new WeixinReceivedContext<UnsubscribeEventReceivedXml>(this, x, isEncrypted);
									handled = await _options.Events.UnsubscribeEventReceived(ctx);
								}
								break;
							case ReceivedEventType.SCAN:
								{
									var x = MyvasXmlConvert.DeserializeObject<QrscanEventReceivedXml>(xml);
									var ctx = new WeixinReceivedContext<QrscanEventReceivedXml>(this, x, isEncrypted);
									handled = await _options.Events.QrscanEventReceived(ctx);
								}
								break;
							case ReceivedEventType.LOCATION:
								{
									var x = MyvasXmlConvert.DeserializeObject<LocationEventReceivedXml>(xml);
									var ctx = new WeixinReceivedContext<LocationEventReceivedXml>(this, x, isEncrypted);
									handled = await _options.Events.LocationEventReceived(ctx);
								}
								break;
							case ReceivedEventType.CLICK:
								{
									var x = MyvasXmlConvert.DeserializeObject<ClickMenuEventReceivedXml>(xml);
									var ctx = new WeixinReceivedContext<ClickMenuEventReceivedXml>(this, x, isEncrypted);
									handled = await _options.Events.ClickMenuEventReceived(ctx);
								}
								break;
							case ReceivedEventType.VIEW:
								{
									var x = MyvasXmlConvert.DeserializeObject<ViewMenuEventReceivedXml>(xml);
									var ctx = new WeixinReceivedContext<ViewMenuEventReceivedXml>(this, x, isEncrypted);
									handled = await _options.Events.ViewMenuEventReceived(ctx);
								}
								break;
							//case ReceivedEventType.ENTER://已确认被腾讯移除！
							//	{
							//		var x = XmlConvert.DeserializeObject<EnterEventReceivedXml>(xml);
							//		var ctx = new WeixinReceivedContext<EnterEventReceivedXml>(this, x, isEncrypted);
							//		handled = await _options.Events.EnterEventReceived(ctx);
							//	}
							//	break;
							default:
								throw new NotSupportedException($"不支持的事件[{ev.Event.ToString()}]");
						}
					}
					break;
				case ReceivedMsgType.text:
					{
						var x = MyvasXmlConvert.DeserializeObject<TextMessageReceivedXml>(xml);
						var ctx = new WeixinReceivedContext<TextMessageReceivedXml>(this, x, isEncrypted);
						handled = await _options.Events.TextMessageReceived(ctx);
					}
					break;
				case ReceivedMsgType.image:
					{
						var x = MyvasXmlConvert.DeserializeObject<ImageMessageReceivedXml>(xml);
						var ctx = new WeixinReceivedContext<ImageMessageReceivedXml>(this, x, isEncrypted);
						handled = await _options.Events.ImageMessageReceived(ctx);
					}
					break;
				case ReceivedMsgType.voice:
					{
						var x = MyvasXmlConvert.DeserializeObject<VoiceMessageReceivedXml>(xml);
						var ctx = new WeixinReceivedContext<VoiceMessageReceivedXml>(this, x, isEncrypted);
						handled = await _options.Events.VoiceMessageReceived(ctx);
					}
					break;
				case ReceivedMsgType.video:
					{
						var x = MyvasXmlConvert.DeserializeObject<VideoMessageReceivedXml>(xml);
						var ctx = new WeixinReceivedContext<VideoMessageReceivedXml>(this, x, isEncrypted);
						handled = await _options.Events.VideoMessageReceived(ctx);
					}
					break;
				case ReceivedMsgType.shortvideo:
					{
						var x = MyvasXmlConvert.DeserializeObject<ShortVideoMessageReceivedXml>(xml);
						var ctx = new WeixinReceivedContext<ShortVideoMessageReceivedXml>(this, x, isEncrypted);
						handled = await _options.Events.ShortVideoMessageReceived(ctx);
					}
					break;
				case ReceivedMsgType.location:
					{
						var x = MyvasXmlConvert.DeserializeObject<LocationMessageReceivedXml>(xml);
						var ctx = new WeixinReceivedContext<LocationMessageReceivedXml>(this, x, isEncrypted);
						handled = await _options.Events.LocationMessageReceived(ctx);
					}
					break;
				case ReceivedMsgType.link:
					{
						var x = MyvasXmlConvert.DeserializeObject<LinkMessageReceivedXml>(xml);
						var ctx = new WeixinReceivedContext<LinkMessageReceivedXml>(this, x, isEncrypted);
						handled = await _options.Events.LinkMessageReceived(ctx);
					}
					break;
				default:
					throw new NotSupportedException($"不支持的信息类型[{received.MsgType.ToString()}]");
			}

			await Task.FromResult(0);
			return handled ? WeixinMessageHandleResult.Handle() : WeixinMessageHandleResult.Fail("未处理");
		}

		public async Task WriteAsync(object o)
		{
			var s = MyvasXmlConvert.SerializeObject(o);
			_logger.LogDebug("Response Body({0}): {1}", s?.Length, s);

			//HttpContext.Response.Clear();
			HttpContext.Response.ContentType = "text/plain;charset=utf-8";

			var timestamp = WeixinTimestampHelper.FromUtcTime(DateTime.UtcNow);
			var nonce = new Random().Next(123456789, 987654321);
			var encrypted = _encryptor.Encrypt(s, timestamp.ToString(), nonce.ToString());
			_logger.LogDebug("Encrypted Response Body({0}): {1}", encrypted?.Length, encrypted);

			await HttpContext.Response.WriteAsync(encrypted);
		}
	}
}
