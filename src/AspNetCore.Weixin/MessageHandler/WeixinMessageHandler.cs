using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace AspNetCore.Weixin
{
    public static class EventHandlerExtensions
    {
        public static void Raise<TEventArgs>(this EventHandler<TEventArgs> handler,
            object sender, TEventArgs args)
            where TEventArgs : EventArgs
        {
            handler?.Invoke(sender, args);
        }
    }

    /// <summary>
    /// 微信请求的集中处理方法
    /// </summary>
    public class WeixinMessageHandler
    {
        private HttpContext _context;
        private WeixinWelcomePageOptions _options;
        private ILogger _logger;
        protected WeixinMessageHandleResult InitializeResult { get; set; }
        public async Task<bool> InitializeAsync(
            WeixinWelcomePageOptions options, 
            HttpContext context, 
            ILogger logger)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

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
        public Task<WeixinMessageHandleResult> HandleOnceAsync()
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
        public async Task<WeixinMessageHandleResult> HandleAsync()
        {
            bool handled = false;

            var xml = new StreamReader(_context.Request.Body).ReadToEnd();
            _logger.LogDebug("Request Body({0}): {1}", xml?.Length, xml);

            var received = XmlConvert.DeserializeObject<ReceivedEventArgs>(xml);
            switch (received.MsgType)
            {
                case ReceivedMsgType.@event:
                    {
                        var ev = XmlConvert.DeserializeObject<EventReceivedEventArgs>(xml);
                        switch (ev.Event)
                        {
                            case ReceivedEventType.subscribe:
                                {
                                    var x = XmlConvert.DeserializeObject<SubscribeEventReceivedEventArgs>(xml);
                                    var ctx = new WeixinReceivedContext<SubscribeEventReceivedEventArgs>(this, x);
                                    handled = await _options.Events.SubscribeEventReceived(ctx);
                                }
                                break;
                            case ReceivedEventType.unsubscribe:
                                {
                                    var x = XmlConvert.DeserializeObject<UnsubscribeEventReceivedEventArgs>(xml);
                                    var ctx = new WeixinReceivedContext<UnsubscribeEventReceivedEventArgs>(this, x);
                                    handled = await _options.Events.UnsubscribeEventReceived(ctx);
                                }
                                break;
                            case ReceivedEventType.SCAN:
                                {
                                    var x = XmlConvert.DeserializeObject<QrscanEventReceivedEventArgs>(xml);
                                    var ctx = new WeixinReceivedContext<QrscanEventReceivedEventArgs>(this, x);
                                    handled = await _options.Events.QrscanEventReceived(ctx);
                                }
                                break;
                            case ReceivedEventType.LOCATION:
                                {
                                    var x = XmlConvert.DeserializeObject<LocationEventReceivedEventArgs>(xml);
                                    var ctx = new WeixinReceivedContext<LocationEventReceivedEventArgs>(this, x);
                                    handled = await _options.Events.LocationEventReceived(ctx);
                                }
                                break;
                            case ReceivedEventType.CLICK:
                                {
                                    var x = XmlConvert.DeserializeObject<ClickMenuEventReceivedEventArgs>(xml);
                                    var ctx = new WeixinReceivedContext<ClickMenuEventReceivedEventArgs>(this, x);
                                    handled = await _options.Events.ClickMenuEventReceived(ctx);
                                }
                                break;
                            case ReceivedEventType.VIEW:
                                {
                                    var x = XmlConvert.DeserializeObject<ViewMenuEventReceivedEventArgs>(xml);
                                    var ctx = new WeixinReceivedContext<ViewMenuEventReceivedEventArgs>(this, x);
                                    handled = await _options.Events.ViewMenuEventReceived(ctx);
                                }
                                break;
                            case ReceivedEventType.ENTER:
                                {
                                    var x = XmlConvert.DeserializeObject<EnterEventReceivedEventArgs>(xml);
                                    var ctx = new WeixinReceivedContext<EnterEventReceivedEventArgs>(this, x);
                                    handled = await _options.Events.EnterEventReceived(ctx);
                                }
                                break;
                            default:
                                throw new NotSupportedException($"不支持的事件[{ev.Event.ToString()}]");
                        }
                    }
                    break;
                case ReceivedMsgType.text:
                    {
                        var x = XmlConvert.DeserializeObject<TextMessageReceivedEventArgs>(xml);
                        var ctx = new WeixinReceivedContext<TextMessageReceivedEventArgs>(this, x);
                        handled = await _options.Events.TextMessageReceived(ctx);
                    }
                    break;
                case ReceivedMsgType.image:
                    {
                        var x = XmlConvert.DeserializeObject<ImageMessageReceivedEventArgs>(xml);
                        var ctx = new WeixinReceivedContext<ImageMessageReceivedEventArgs>(this, x);
                        handled = await _options.Events.ImageMessageReceived(ctx);
                    }
                    break;
                case ReceivedMsgType.link:
                    {
                        var x = XmlConvert.DeserializeObject<LinkMessageReceivedEventArgs>(xml);
                        var ctx = new WeixinReceivedContext<LinkMessageReceivedEventArgs>(this, x);
                        handled = await _options.Events.LinkMessageReceived(ctx);
                    }
                    break;
                case ReceivedMsgType.location:
                    {
                        var x = XmlConvert.DeserializeObject<LocationMessageReceivedEventArgs>(xml);
                        var ctx = new WeixinReceivedContext<LocationMessageReceivedEventArgs>(this, x);
                        handled = await _options.Events.LocationMessageReceived(ctx);
                    }
                    break;
                case ReceivedMsgType.voice:
                    {
                        var x = XmlConvert.DeserializeObject<VoiceMessageReceivedEventArgs>(xml);
                        var ctx = new WeixinReceivedContext<VoiceMessageReceivedEventArgs>(this, x);
                        handled = await _options.Events.VoiceMessageReceived(ctx);
                    }
                    break;
                case ReceivedMsgType.video:
                    {
                        var x = XmlConvert.DeserializeObject<VideoMessageReceivedEventArgs>(xml);
                        var ctx = new WeixinReceivedContext<VideoMessageReceivedEventArgs>(this, x);
                        handled = await _options.Events.VideoMessageReceived(ctx);
                    }
                    break;
                case ReceivedMsgType.shortvideo:
                    {
                        var x = XmlConvert.DeserializeObject<ShortVideoMessageReceivedEventArgs>(xml);
                        var ctx = new WeixinReceivedContext<ShortVideoMessageReceivedEventArgs>(this, x);
                        handled = await _options.Events.ShortVideoMessageReceived(ctx);
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
            var s = XmlConvert.SerializeObject(o);
            _logger.LogDebug("Response Body({0}): {1}", s?.Length, s);

            _context.Response.Clear();
            _context.Response.ContentType = "text/plain;charset=utf-8";
            await _context.Response.WriteAsync(s);
        }
    }
}
