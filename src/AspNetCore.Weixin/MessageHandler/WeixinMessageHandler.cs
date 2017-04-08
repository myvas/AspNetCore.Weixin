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
        #region replaced by _options.Events
        //#region 事件
        ///// <summary>
        ///// 收到用户进入微信号（含订阅号和服务号）会话
        ///// </summary>
        //[Obsolete("此事件似乎已被腾讯拿掉！请考虑使用其他方式实现，例如LocationEventReceived(自动上报当前位置)。")]
        //public event EventHandler<EnterEventReceivedEventArgs> EnterEventReceived;
        ///// <summary>
        ///// 收到订阅事件。或，未订阅用户扫描带场景码事件（先订阅，再处理场景）。
        ///// </summary>
        //public event EventHandler<SubscribeEventReceivedEventArgs> SubscribeEventReceived;

        ///// <summary>
        ///// 收到二维码扫描事件
        ///// <para>通常意味着用户通过扫描一个二维码订阅指定微信号服务。建议充分利用事件中的SceneId(场景码)区分各种不同来源的用户群。</para>
        ///// </summary>
        //public event EventHandler<QrscanEventReceivedEventArgs> QrscanEventReceived;
        ///// <summary>
        ///// 收到退订事件
        ///// </summary>
        //public event EventHandler<UnsubscribeEventReceivedEventArgs> UnsubscribeEventReceived;
        ///// <summary>
        ///// 收到点击菜单（拉取消息）事件
        ///// </summary>
        //public event EventHandler<ClickMenuEventReceivedEventArgs> ClickMenuEventReceived;
        ///// <summary>
        ///// 收到点击菜单（跳转链接）事件
        ///// </summary>
        //public event EventHandler<ViewMenuEventReceivedEventArgs> ViewMenuEventReceived;
        ///// <summary>
        ///// 收到自动上报当前位置信息
        ///// <para>与<see cref="LocationMessageReceived"/>不同，本事件是自动上报当前位置，而后者是用户在地图上选择一个地点后上传的一条位置信息。</para>
        ///// </summary>
        ///// <seealso cref="LocationMessageReceived"/>
        //public event EventHandler<LocationEventReceivedEventArgs> LocationEventReceived;
        //#endregion
        //#region 消息
        ///// <summary>
        ///// 收到位置信息
        ///// <para>与<see cref="LocationEventReceived"/>不同，本事件是用户在地图上选择一个地点后上传的一条位置信息，而后者是自动上报当前位置。</para>
        ///// </summary>
        ///// <seealso cref="LocationEventReceived"/>
        //public event EventHandler<LocationMessageReceivedEventArgs> LocationMessageReceived;
        ///// <summary>
        ///// 收到文本信息
        ///// </summary>
        //public event EventHandler<TextMessageReceivedEventArgs> TextMessageReceived;
        ///// <summary>
        ///// 收到图片信息
        ///// </summary>
        //public event EventHandler<ImageMessageReceivedEventArgs> ImageMessageReceived;
        ///// <summary>
        ///// 收到链接信息
        ///// </summary>
        //public event EventHandler<LinkMessageReceivedEventArgs> LinkMessageReceived;
        ///// <summary>
        ///// 收到语音信息
        ///// </summary>
        //public event EventHandler<VoiceMessageReceivedEventArgs> VoiceMessageReceived;
        ///// <summary>
        ///// 收到视频信息
        ///// </summary>
        //public event EventHandler<VideoMessageReceivedEventArgs> VideoMessageReceived;
        ///// <summary>
        ///// 收到小视频信息
        ///// </summary>
        //public event EventHandler<ShortVideoMessageReceivedEventArgs> ShortVideoMessageReceived;
        //#endregion
        #endregion

        private HttpContext _context;
        private WeixinWelcomePageOptions _options;
        private ILogger _logger;
        protected WeixinMessageHandleResult InitializeResult { get; set; }
        public async Task InitializeAsync(WeixinWelcomePageOptions options, HttpContext context, ILogger logger)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            context.Response.OnStarting(OnStartingCallback, this);
            {
                InitializeResult = await HandleOnceAsync();
                if (InitializeResult?.Handled == true)
                {
                    return;
                }
                if (InitializeResult?.Failure != null)
                {
                    _logger.LogWarning(0, InitializeResult.Failure, InitializeResult.Failure.Message);
                }
            }
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
            WeixinMessageHandleResult result = null;

            var xml = new StreamReader(_context.Request.Body).ReadToEnd();
            _logger.LogDebug("Request Body({1}): {0}", xml, xml?.Length);

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
                                    await _options.Events.SubscribeEventReceived(ctx);
                                }
                                break;
                            case ReceivedEventType.unsubscribe:
                                {
                                    var x = XmlConvert.DeserializeObject<UnsubscribeEventReceivedEventArgs>(xml);
                                    var ctx = new WeixinReceivedContext<UnsubscribeEventReceivedEventArgs>(this, x);
                                    await _options.Events.UnsubscribeEventReceived(ctx);
                                }
                                break;
                            case ReceivedEventType.SCAN:
                                {
                                    var x = XmlConvert.DeserializeObject<QrscanEventReceivedEventArgs>(xml);
                                    var ctx = new WeixinReceivedContext<QrscanEventReceivedEventArgs>(this, x);
                                    await _options.Events.QrscanEventReceived(ctx);
                                }
                                break;
                            case ReceivedEventType.LOCATION:
                                {
                                    var x = XmlConvert.DeserializeObject<LocationEventReceivedEventArgs>(xml);
                                    var ctx = new WeixinReceivedContext<LocationEventReceivedEventArgs>(this, x);
                                    await _options.Events.LocationEventReceived(ctx);
                                }
                                break;
                            case ReceivedEventType.CLICK:
                                {
                                    var x = XmlConvert.DeserializeObject<ClickMenuEventReceivedEventArgs>(xml);
                                    var ctx = new WeixinReceivedContext<ClickMenuEventReceivedEventArgs>(this, x);
                                    await _options.Events.ClickMenuEventReceived(ctx);
                                }
                                break;
                            case ReceivedEventType.VIEW:
                                {
                                    var x = XmlConvert.DeserializeObject<ViewMenuEventReceivedEventArgs>(xml);
                                    var ctx = new WeixinReceivedContext<ViewMenuEventReceivedEventArgs>(this, x);
                                    await _options.Events.ViewMenuEventReceived(ctx);
                                }
                                break;
                            case ReceivedEventType.ENTER:
                                {
                                    var x = XmlConvert.DeserializeObject<EnterEventReceivedEventArgs>(xml);
                                    var ctx = new WeixinReceivedContext<EnterEventReceivedEventArgs>(this, x);
                                    await _options.Events.EnterEventReceived(ctx);
                                }
                                break;
                            default:
                                throw new NotSupportedException($"不支持的事件[{ev.Event.ToString()}]");
                                break;
                        }
                    }
                    break;
                case ReceivedMsgType.text:
                    {
                        var x = XmlConvert.DeserializeObject<TextMessageReceivedEventArgs>(xml);
                        var ctx = new WeixinReceivedContext<TextMessageReceivedEventArgs>(this, x);
                        await _options.Events.TextMessageReceived(ctx);
                    }
                    break;
                case ReceivedMsgType.image:
                    {
                        var x = XmlConvert.DeserializeObject<ImageMessageReceivedEventArgs>(xml);
                        var ctx = new WeixinReceivedContext<ImageMessageReceivedEventArgs>(this, x);
                        await _options.Events.ImageMessageReceived(ctx);
                    }
                    break;
                case ReceivedMsgType.link:
                    {
                        var x = XmlConvert.DeserializeObject<LinkMessageReceivedEventArgs>(xml);
                        var ctx = new WeixinReceivedContext<LinkMessageReceivedEventArgs>(this, x);
                        await _options.Events.LinkMessageReceived(ctx);
                    }
                    break;
                case ReceivedMsgType.location:
                    {
                        var x = XmlConvert.DeserializeObject<LocationMessageReceivedEventArgs>(xml);
                        var ctx = new WeixinReceivedContext<LocationMessageReceivedEventArgs>(this, x);
                        await _options.Events.LocationMessageReceived(ctx);
                    }
                    break;
                case ReceivedMsgType.voice:
                    {
                        var x = XmlConvert.DeserializeObject<VoiceMessageReceivedEventArgs>(xml);
                        var ctx = new WeixinReceivedContext<VoiceMessageReceivedEventArgs>(this, x);
                        await _options.Events.VoiceMessageReceived(ctx);
                    }
                    break;
                case ReceivedMsgType.video:
                    {
                        var x = XmlConvert.DeserializeObject<VideoMessageReceivedEventArgs>(xml);
                        var ctx = new WeixinReceivedContext<VideoMessageReceivedEventArgs>(this, x);
                        await _options.Events.VideoMessageReceived(ctx);
                    }
                    break;
                case ReceivedMsgType.shortvideo:
                    {
                        var x = XmlConvert.DeserializeObject<ShortVideoMessageReceivedEventArgs>(xml);
                        var ctx = new WeixinReceivedContext<ShortVideoMessageReceivedEventArgs>(this, x);
                        await _options.Events.ShortVideoMessageReceived(ctx);
                    }
                    break;
                default:
                    throw new NotSupportedException($"不支持的信息类型[{received.MsgType.ToString()}]");
                    break;
            }

            await Task.FromResult(0);
            result = WeixinMessageHandleResult.Handle();
            return result;
        }

        public async Task WriteAsync(object o)
        {
            var s = XmlConvert.SerializeObject(0);
            _context.Response.Clear();
            _context.Response.ContentType = "text/plain;charset=utf-8";
            await _context.Response.WriteAsync(s);
        }
    }
}
