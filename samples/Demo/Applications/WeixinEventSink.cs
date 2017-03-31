using AspNetCore.Weixin;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Demo.Applications
{
    public class WeixinEventSink
    {
        private readonly ILogger<WeixinEventSink> _logger;
        public WeixinEventSink(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory?.CreateLogger<WeixinEventSink>() ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public bool OnTextMessageReceived(object sender, TextMessageReceivedEventArgs e)
        {
            var messageHandler = sender as MessageHandler<MessageContext>;
            var responseMessage = messageHandler.CreateResponseMessage<ResponseMessageText>();
            {
                var s = XmlConvert.SerializeObject(e);
                var result = new StringBuilder();
                result.AppendFormat("您刚才发送了文本信息：{0}\r\n\r\n{1}", e.Content, s);

                responseMessage.Content = result.ToString();
            }
            messageHandler.ResponseMessage = responseMessage;

            return true;
        }

        public bool OnLinkMessageReceived(object sender, LinkMessageReceivedEventArgs e)
        {
            _logger.LogInformation($"OnLinkMessageReceived: {e.Url}");

            var messageHandler = sender as MessageHandler<MessageContext>;
            var responseMessage = messageHandler.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = string.Format(@"您发送了一条链接信息：
            Title：{0}
            Description:{1}
            Url:{2}", e.Title, e.Description, e.Url);
            messageHandler.ResponseMessage = responseMessage;

            return true;
        }

        public bool OnVideoMessageReceived(object sender, VideoMessageReceivedEventArgs e)
        {
            var messageHandler = sender as MessageHandler<MessageContext>;
            var responseMessage = messageHandler.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "您发送了一条视频信息，ID：" + e.MediaId;
            messageHandler.ResponseMessage = responseMessage;

            return true;
        }

        public bool OnVoiceMessageReceived(object sender, VoiceMessageReceivedEventArgs e)
        {
            var messageHandler = sender as MessageHandler<MessageContext>;
            var responseMessage = messageHandler.CreateResponseMessage<ResponseMessageMusic>();
            responseMessage.Music.MusicUrl = e.MediaId;
            responseMessage.Music.Title = "语音";
            responseMessage.Music.Description = "这里是一条语音消息";
            messageHandler.ResponseMessage = responseMessage;

            return true;
        }

        public bool OnImageMessageReceived(object sender, ImageMessageReceivedEventArgs e)
        {
            var messageHandler = sender as MessageHandler<MessageContext>;
            var responseMessage = messageHandler.CreateResponseMessage<ResponseMessageNews>();
            responseMessage.Articles.Add(new Article()
            {
                Title = "您刚才发送了图片信息",
                Description = "您发送的图片将会显示在边上",
                PicUrl = e.PicUrl,
                Url = "http://wx.demo.com"
            });
            responseMessage.Articles.Add(new Article()
            {
                Title = "第二条",
                Description = "第二条带连接的内容",
                PicUrl = e.PicUrl,
                Url = "http://wx.demo.com"
            });
            messageHandler.ResponseMessage = responseMessage;

            return true;
        }

        public bool OnLocationMessageReceived(object sender, LocationMessageReceivedEventArgs e)
        {
            var messageHandler = sender as MessageHandler<MessageContext>;
            var responseMessage = messageHandler.CreateResponseMessage<ResponseMessageNews>();

            var markersList = new List<GoogleMapMarkers>();
            markersList.Add(new GoogleMapMarkers()
            {
                Latitude = e.Latitude,
                Longitude = e.Longitude,
                Color = "red",
                Label = "S",
                Size = GoogleMapMarkerSize.Default,
            });
            var mapSize = "480x600";
            var mapUrl = GoogleMapHelper.GetGoogleStaticMap(19 /*requestMessage.Scale*//*微信和GoogleMap的Scale不一致，这里建议使用固定值*/,
                                                            markersList, mapSize);
            responseMessage.Articles.Add(new Article()
            {
                Description = string.Format("您刚才发送了地理位置信息。Location_X：{0}，Location_Y：{1}，Scale：{2}，标签：{3}",
                              e.Latitude, e.Longitude,
                              e.Scale, e.Label),
                PicUrl = mapUrl,
                Title = "定位地点周边地图",
                Url = mapUrl
            });
            responseMessage.Articles.Add(new Article()
            {
                Title = "AspNetCore.Weixin",
                Description = "AspNetCore.Weixin",
                PicUrl = "http://wx.demo.com/logo.jpg",
                Url = "http://wx.demo.com"
            });

            messageHandler.ResponseMessage = responseMessage;

            return true;
        }

        public bool OnLocationEventReceived(object sender, LocationEventReceivedEventArgs e)
        {
            //这里是微信客户端（通过微信服务器）自动发送过来的位置信息
            var messageHandler = sender as MessageHandler<MessageContext>;
            var responseMessage = messageHandler.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = string.Format("刚刚上报了一条定位信息：(Lat={0},Lon={1},Prc={2})",
                e.Latitude, e.Longitude, e.Precision);
            messageHandler.ResponseMessage = responseMessage;

            return true;
        }

        public bool OnClickMenuEventReceived(object sender, ClickMenuEventReceivedEventArgs e)
        {
            _logger.LogDebug("点击了子菜单按钮({0}): {1}", e.FromUserName, e.MenuItemKey);

            var messageHandler = sender as MessageHandler<MessageContext>;
            var responseMessage = messageHandler.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = string.Format("点击了子菜单按钮({0}): {1}", e.FromUserName, e.MenuItemKey);
            messageHandler.ResponseMessage = responseMessage;

            return true;
        }

        public bool OnViewMenuEventReceived(object sender, ViewMenuEventReceivedEventArgs e)
        {
            _logger.LogDebug("点击了子菜单按钮({0}): {1}", e.FromUserName, e.Url);

            var messageHandler = sender as MessageHandler<MessageContext>;
            var responseMessage = messageHandler.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = string.Format("点击了子菜单按钮({0}): {1}", e.FromUserName, e.Url);
            messageHandler.ResponseMessage = responseMessage;

            return true;
        }

        public bool OnUnsubscribeEventReceived(object sender, UnsubscribeEventReceivedEventArgs e)
        {
            _logger.LogDebug("Unsubscribe({0})", e.FromUserName);

            var messageHandler = sender as MessageHandler<MessageContext>;
            var responseMessage = messageHandler.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = string.Format("Unsubscribe({0})", e.FromUserName);
            messageHandler.ResponseMessage = responseMessage;

            return true;
        }

        public bool OnSubscribeEventReceived(object sender, SubscribeEventReceivedEventArgs e)
        {
            var messageHandler = sender as MessageHandler<MessageContext>;
            if (string.IsNullOrWhiteSpace(e.EventKey))
            {
                _logger.LogDebug("Subscribe: from:{0}", e.FromUserName);

                var responseMessage = messageHandler.CreateResponseMessage<ResponseMessageNews>();
                responseMessage.Articles.Add(new Article()
                {
                    Title = "欢迎体验AspNetCore.Weixin演示系统",
                    Description = "由AspNetCore.Weixin提供",
                    PicUrl = "https://mp.weixin.qq.com/cgi-bin/getimgdata?mode=large&source=file&fileId=200121314%3E&token=977619473&lang=zh_CN",
                    Url = "http://wx.demo.com"
                });
                messageHandler.ResponseMessage = responseMessage;
            }
            else
            {
                _logger.LogDebug("Subscribe w/ scene({0}): {1}, {2}", e.FromUserName, e.EventKey, e.Ticket);


                var responseMessage = messageHandler.CreateResponseMessage<ResponseMessageNews>();
                responseMessage.Articles.Add(new Article()
                {
                    Title = "欢迎体验AspNetCore.Weixin演示系统",
                    Description = "由AspNetCore.Weixin提供。此消息带场景({e.EventKey}, {e.Ticket})",
                    PicUrl = "https://mp.weixin.qq.com/cgi-bin/getimgdata?mode=large&source=file&fileId=200121314%3E&token=977619473&lang=zh_CN",
                    Url = "http://wx.demo.com"
                });
                messageHandler.ResponseMessage = responseMessage;
            }

            return true;
        }

        public bool OnQrscanEventReceived(object sender, QrscanEventReceivedEventArgs e)
        {
            _logger.LogDebug("Qrscan({0}): {1}, {2}", e.FromUserName, e.EventKey, e.Ticket);

            var messageHandler = sender as MessageHandler<MessageContext>;
            var responseMessage = messageHandler.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = string.Format("Qrscan({0}): {1}, {2}", e.FromUserName, e.EventKey, e.Ticket);
            messageHandler.ResponseMessage = responseMessage;

            return true;
        }
    }
}
