using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
            if (handler != null)
            {
                handler(sender, args);
            }
        }
    }

    /// <summary>
    /// 微信请求的集中处理方法
    /// </summary>
    public abstract partial class MessageHandler<TC> : IMessageHandler
        where TC : class, IMessageContext, new()
    {
        #region 事件
        /// <summary>
        /// 收到订阅事件
        /// </summary>
        public event EventHandler<SubscribeEventReceivedEventArgs> SubscribeEventReceived;
        /// <summary>
        /// 收到二维码扫描事件
        /// <para>通常意味着用户通过扫描一个二维码订阅指定微信号服务。建议充分利用事件中的SceneId(场景码)区分各种不同来源的用户群。</para>
        /// </summary>
        public event EventHandler<QrscanEventReceivedEventArgs> QrscanEventReceived;
        /// <summary>
        /// 收到退订事件
        /// </summary>
        public event EventHandler<UnsubscribeEventReceivedEventArgs> UnsubscribeEventReceived;
        /// <summary>
        /// 收到点击菜单（拉取消息）事件
        /// </summary>
        public event EventHandler<ClickMenuEventReceivedEventArgs> ClickMenuEventReceived;
        /// <summary>
        /// 收到点击菜单（跳转链接）事件
        /// </summary>
        public event EventHandler<ViewMenuEventReceivedEventArgs> ViewMenuEventReceived;
        /// <summary>
        /// 收到自动上报当前位置信息
        /// <para>与<see cref="LocationMessageReceived"/>不同，本事件是自动上报当前位置，而后者是用户在地图上选择一个地点后上传的一条位置信息。</para>
        /// </summary>
        /// <seealso cref="LocationMessageReceived"/>
        public event EventHandler<LocationEventReceivedEventArgs> LocationEventReceived;

        /// <summary>
        /// 收到位置信息
        /// <para>与<see cref="LocationEventReceived"/>不同，本事件是用户在地图上选择一个地点后上传的一条位置信息，而后者是自动上报当前位置。</para>
        /// </summary>
        /// <seealso cref="LocationEventReceived"/>
        public event EventHandler<LocationMessageReceivedEventArgs> LocationMessageReceived;
        /// <summary>
        /// 收到文本信息
        /// </summary>
        public event EventHandler<TextMessageReceivedEventArgs> TextMessageReceived;
        /// <summary>
        /// 收到图片信息
        /// </summary>
        public event EventHandler<ImageMessageReceivedEventArgs> ImageMessageReceived;
        /// <summary>
        /// 收到链接信息
        /// </summary>
        public event EventHandler<LinkMessageReceivedEventArgs> LinkMessageReceived;
        /// <summary>
        /// 收到语音信息
        /// </summary>
        public event EventHandler<VoiceMessageReceivedEventArgs> VoiceMessageReceived;
        /// <summary>
        /// 收到视频信息
        /// </summary>
        public event EventHandler<VideoMessageReceivedEventArgs> VideoMessageReceived;

        /// <summary>
        /// 收到用户进入微信号（含订阅号和服务号）会话
        /// <para>此事件似乎已被腾讯拿掉！</para>
        /// </summary>
        [Obsolete("此事件似乎已被腾讯拿掉！请考虑使用其他方式实现，例如LocationEventReceived(自动上报当前位置)。")]
        public event EventHandler<EventReceivedEventArgs> EnterEventReceived;
        #endregion

        /// <summary>
        /// 上下文
        /// </summary>
        public static WeixinContext<TC> GlobalWeixinContext = new WeixinContext<TC>();

        /// <summary>
        /// 全局消息上下文
        /// </summary>
        public WeixinContext<TC> WeixinContext
        {
            get { return GlobalWeixinContext; }
        }

        /// <summary>
        /// 当前用户消息上下文
        /// </summary>
        public TC CurrentMessageContext
        {
            get { return WeixinContext.GetMessageContext(RequestMessage); }
        }

        /// <summary>
        /// 发送者用户名（OpenId）
        /// </summary>
        public string WeixinOpenId
        {
            get
            {
                if (RequestMessage != null)
                {
                    return RequestMessage.FromUserName;
                }
                return null;
            }
        }

        /// <summary>
        /// 取消执行Execute()方法。一般在OnExecuting()中用于临时阻止执行Execute()。
        /// 默认为False。
        /// 如果在执行OnExecuting()执行前设为True，则所有OnExecuting()、Execute()、OnExecuted()代码都不会被执行。
        /// 如果在执行OnExecuting()执行过程中设为True，则后续Execute()及OnExecuted()代码不会被执行。
        /// 建议在设为True的时候，给ResponseMessage赋值，以返回友好信息。
        /// </summary>
        public bool CancelExcute { get; set; }

        /// <summary>
        /// 在构造函数中转换得到原始XML数据
        /// </summary>
        public XDocument RequestDocument { get; set; }

        /// <summary>
        /// 根据ResponseMessageBase获得转换后的ResponseDocument
        /// 注意：这里每次请求都会根据当前的ResponseMessageBase生成一次，如需重用此数据，建议使用缓存或局部变量
        /// </summary>
        public XDocument ResponseDocument
        {
            get
            {
                if (ResponseMessage == null)
                {
                    return null;
                }
                return Extensions.ConvertEntityToXml(ResponseMessage as ResponseMessageBase);
            }
        }

        //protected Stream InputStream { get; set; }
        /// <summary>
        /// 请求实体
        /// </summary>
        public IRequestMessageBase RequestMessage { get; set; }
        /// <summary>
        /// 响应实体
        /// 正常情况下只有当执行Execute()方法后才可能有值。
        /// 也可以结合Cancel，提前给ResponseMessage赋值。
        /// </summary>
        public IResponseMessageBase ResponseMessage { get; set; }

        /// <summary>
        /// 是否使用了MessageAgent代理
        /// </summary>
        public bool UsedMessageAgent { get; set; }

        protected MessageHandler(Stream inputStream, int maxRecordCount = 0)
        {
            WeixinContext.MaxRecordCount = maxRecordCount;
            using (XmlReader xr = XmlReader.Create(inputStream))
            {
                RequestDocument = XDocument.Load(xr);
                Init(RequestDocument);
            }
        }

        protected MessageHandler(XDocument requestDocument, int maxRecordCount = 0)
        {
            WeixinContext.MaxRecordCount = maxRecordCount;
            Init(requestDocument);
        }

        private void Init(XDocument requestDocument)
        {
            RequestDocument = requestDocument;
            RequestMessage = RequestMessageFactory.GetRequestEntity(RequestDocument);

            //记录上下文
            if (WeixinContextGlobal.UseWeixinContext)
            {
                WeixinContext.InsertMessage(RequestMessage);
            }
        }

        /// <summary>
        /// 根据当前的RequestMessage创建指定类型的ResponseMessage
        /// </summary>
        /// <typeparam name="TR">基于ResponseMessageBase的响应消息类型</typeparam>
        /// <returns></returns>
        public TR CreateResponseMessage<TR>() where TR : ResponseMessageBase
        {
            if (RequestMessage == null)
            {
                return null;
            }

            return RequestMessage.CreateResponseMessage<TR>();
        }

        /// <summary>
        /// 执行微信请求
        /// </summary>
        public void Execute()
        {
            if (CancelExcute)
            {
                return;
            }

            OnExecuting();

            if (CancelExcute)
            {
                return;
            }

            try
            {
                if (RequestMessage == null)
                {
                    return;
                }

                switch (RequestMessage.MsgType)
                {
                    case RequestMsgType.Text:
                        {
                            var requestMessage = RequestMessage as RequestMessageText;
                            var x = OnTextOrEventRequest(requestMessage);
                            if (x == null) OnTextRequest(requestMessage);
                            if (ResponseMessage == null) ResponseMessage = x;
                        }
                        break;
                    case RequestMsgType.Location:
                        OnLocationRequest(RequestMessage as RequestMessageLocation);
                        break;
                    case RequestMsgType.Image:
                        OnImageRequest(RequestMessage as RequestMessageImage);
                        break;
                    case RequestMsgType.Voice:
                        OnVoiceRequest(RequestMessage as RequestMessageVoice);
                        break;
                    case RequestMsgType.Video:
                        OnVideoRequest(RequestMessage as RequestMessageVideo);
                        break;
                    case RequestMsgType.Event:
                        {
                            var requestMessageText = (RequestMessage as IRequestMessageEventBase).ConvertToRequestMessageText();
                            var x = OnTextOrEventRequest(requestMessageText);
                            if (x == null) OnEventRequest(RequestMessage as IRequestMessageEventBase);
                            if (ResponseMessage == null) ResponseMessage = x;
                        }
                        break;
                    default:
                        throw new WeixinUnknownRequestMsgTypeException("未知的MsgType请求类型", null);
                }

                //记录上下文
                if (WeixinContextGlobal.UseWeixinContext && ResponseMessage != null)
                {
                    WeixinContext.InsertMessage(ResponseMessage);
                }
            }
            finally
            {
                OnExecuted();
            }
        }

        public virtual void OnExecuting()
        {
        }

        public virtual void OnExecuted()
        {
        }

        /// <summary>
        /// 默认返回消息（当任何OnXX消息没有被重写，都将自动返回此默认消息）
        /// </summary>
        public abstract IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage);
        //{
        //    例如可以这样实现：
        //    var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
        //    responseMessage.Content = "您发送的消息类型暂未被识别。";
        //    return responseMessage;
        //}

        /// <summary>
        /// 预处理文字或事件类型请求。
        /// 这个请求是一个比较特殊的请求，通常用于统一处理来自文字或菜单按钮的同一个执行逻辑，
        /// 会在执行OnTextRequest或OnEventRequest之前触发，具有以下一些特征：
        /// 1、如果返回null，则继续执行OnTextRequest或OnEventRequest
        /// 2、如果返回不为null，则终止执行OnTextRequest或OnEventRequest，返回最终ResponseMessage
        /// 3、如果是事件，则会将RequestMessageEvent自动转为RequestMessageText类型，其中RequestMessageText.Content就是RequestMessageEvent.EventKey
        /// </summary>
        public virtual IResponseMessageBase OnTextOrEventRequest(RequestMessageText requestMessage)
        {
            return null;
        }

        /// <summary>
        /// 文字类型请求
        /// </summary>
        public void OnTextRequest(RequestMessageText requestMessage)
        {
            TextMessageReceived.Raise(this, new TextMessageReceivedEventArgs()
            {
                FromUserName = requestMessage.FromUserName,
                CreateTime = requestMessage.CreateTime,
                Content = requestMessage.Content
            });
        }

        /// <summary>
        /// 位置类型请求
        /// </summary>
        public void OnLocationRequest(RequestMessageLocation requestMessage)
        {
            LocationMessageReceived.Raise(this, new LocationMessageReceivedEventArgs()
            {
                FromUserName = requestMessage.FromUserName,
                CreateTime = requestMessage.CreateTime,
                Latitude = requestMessage.Latitude,
                Longitude = requestMessage.Longitude,
                Scale = requestMessage.Scale,
                Label = requestMessage.Label
            });
        }

        /// <summary>
        /// 图片类型请求
        /// </summary>
        public void OnImageRequest(RequestMessageImage requestMessage)
        {
            ImageMessageReceived.Raise(this, new ImageMessageReceivedEventArgs()
            {
                FromUserName = requestMessage.FromUserName,
                CreateTime = requestMessage.CreateTime,
                PicUrl = requestMessage.PicUrl,
                MediaId = requestMessage.MediaId
            });
        }

        /// <summary>
        /// 语音类型请求
        /// </summary>
        public void OnVoiceRequest(RequestMessageVoice requestMessage)
        {
            VoiceMessageReceived.Raise(this, new VoiceMessageReceivedEventArgs()
            {
                FromUserName = requestMessage.FromUserName,
                CreateTime = requestMessage.CreateTime,
                MediaId = requestMessage.MediaId,
                Format = requestMessage.Format
            });
        }


        /// <summary>
        /// 视频类型请求
        /// </summary>
        public void OnVideoRequest(RequestMessageVideo requestMessage)
        {
            VideoMessageReceived.Raise(this, new VideoMessageReceivedEventArgs()
            {
                FromUserName = requestMessage.FromUserName,
                CreateTime = requestMessage.CreateTime,
                MediaId = requestMessage.MediaId,
                ThumbMediaId = requestMessage.ThumbMediaId
            });
        }


        /// <summary>
        /// 链接消息类型请求
        /// </summary>
        public void OnLinkRequest(RequestMessageLink requestMessage)
        {
            LinkMessageReceived.Raise(this, new LinkMessageReceivedEventArgs()
            {
                FromUserName = requestMessage.FromUserName,
                CreateTime = requestMessage.CreateTime,
                Url = requestMessage.Url,
                Title = requestMessage.Title,
                Description = requestMessage.Description
            });
        }

        /// <summary>
        /// Event事件类型请求
        /// </summary>
        public void OnEventRequest(IRequestMessageEventBase requestMessage)
        {
            var strongRequestMessage = RequestMessage as IRequestMessageEventBase;
            switch (strongRequestMessage.Event)
            {
                case WeixinEvent.ENTER:
                    OnEvent_EnterRequest(RequestMessage as RequestMessageEventEnter);
                    break;
                case WeixinEvent.LOCATION://自动发送的用户当前位置
                    OnEvent_LocationRequest(RequestMessage as RequestMessageEventLocation);
                    break;
                case WeixinEvent.subscribe://订阅
                    OnEvent_SubscribeRequest(RequestMessage as RequestMessageEventSubscribe);
                    break;
                case WeixinEvent.unsubscribe://退订
                    OnEvent_UnsubscribeRequest(RequestMessage as RequestMessageEventUnsubscribe);
                    break;
                case WeixinEvent.CLICK://菜单点击
                    OnEvent_ClickRequest(RequestMessage as RequestMessageEventClick);
                    break;
                case WeixinEvent.scan://二维码
                    OnEvent_ScanRequest(RequestMessage as RequestMessageEventScan);
                    break;
                case WeixinEvent.VIEW://URL跳转（view视图）
                    OnEvent_ViewRequest(RequestMessage as RequestMessageEventView);
                    break;
                default:
                    throw new WeixinUnknownRequestMsgTypeException("未知的Event下属请求信息", null);
            }
        }

        #region Event 下属分类

        /// <summary>
        /// Event事件类型请求之ENTER
        /// <remarks>似乎已从微信API中移除??</remarks>
        /// </summary>
        public void OnEvent_EnterRequest(RequestMessageEventEnter requestMessage)
        {
            EnterEventReceived.Raise(this, new EventReceivedEventArgs()
            {
                FromUserName = requestMessage.FromUserName,
                CreateTime = requestMessage.CreateTime
            });
        }

        /// <summary>
        /// Event事件类型请求之LOCATION
        /// </summary>
        public void OnEvent_LocationRequest(RequestMessageEventLocation requestMessage)
        {
            LocationEventReceived.Raise(this, new LocationEventReceivedEventArgs()
            {
                FromUserName = requestMessage.FromUserName,
                CreateTime = requestMessage.CreateTime,
                Latitude = requestMessage.Latitude,
                Longitude = requestMessage.Longitude,
                Precision = requestMessage.Precision,
            });
        }

        /// <summary>
        /// Event事件类型请求之subscribe
        /// </summary>
        public void OnEvent_SubscribeRequest(RequestMessageEventSubscribe requestMessage)
        {
            SubscribeEventReceived.Raise(this, new SubscribeEventReceivedEventArgs()
            {
                FromUserName = requestMessage.FromUserName,
                CreateTime = requestMessage.CreateTime
            });
        }

        /// <summary>
        /// Event事件类型请求之unsubscribe
        /// </summary>
        public void OnEvent_UnsubscribeRequest(RequestMessageEventUnsubscribe requestMessage)
        {
            UnsubscribeEventReceived.Raise(this, new UnsubscribeEventReceivedEventArgs()
            {
                FromUserName = requestMessage.FromUserName,
                CreateTime = requestMessage.CreateTime
            });
        }

        /// <summary>
        /// Event事件类型请求之CLICK
        /// </summary>
        public void OnEvent_ClickRequest(RequestMessageEventClick requestMessage)
        {
            ClickMenuEventReceived.Raise(this, new ClickMenuEventReceivedEventArgs()
            {
                FromUserName = requestMessage.FromUserName,
                CreateTime = requestMessage.CreateTime,
                MenuItemKey = requestMessage.EventKey
            });
        }

        /// <summary>
        /// Event事件类型请求之scan
        /// </summary>
        public void OnEvent_ScanRequest(RequestMessageEventScan requestMessage)
        {
            QrscanEventReceived.Raise(this, new QrscanEventReceivedEventArgs()
            {
                FromUserName = requestMessage.FromUserName,
                CreateTime = requestMessage.CreateTime,
                SceneId = requestMessage.EventKey
            });
        }

        /// <summary>
        /// 事件之URL跳转视图（View）
        /// </summary>
        /// <returns></returns>
        public void OnEvent_ViewRequest(RequestMessageEventView requestMessage)
        {
            ViewMenuEventReceived.Raise(this, new ViewMenuEventReceivedEventArgs()
            {
                FromUserName = requestMessage.FromUserName,
                CreateTime = requestMessage.CreateTime,
                Url = requestMessage.EventKey
            });
        }
        #endregion
    }
}
