using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Myvas.AspNetCore.Weixin
{
    public static class RequestMessageFactory
    {
        //<?xml version="1.0" encoding="utf-8"?>
        //<xml>
        //  <ToUserName><![CDATA[gh_a96a4a619366]]></ToUserName>
        //  <FromUserName><![CDATA[olPjZjsXuQPJoV0HlruZkNzKc91E]]></FromUserName>
        //  <CreateTime>1357986928</CreateTime>
        //  <MsgType><![CDATA[text]]></MsgType>
        //  <Content><![CDATA[中文]]></Content>
        //  <MsgId>5832509444155992350</MsgId>
        //</xml>

        /// <summary>
        /// 获取XDocument转换后的IRequestMessageBase实例。
        /// 如果MsgType不存在，抛出UnknownRequestMsgTypeException异常
        /// </summary>
        /// <returns></returns>
        public static IRequestMessageBase GetRequestEntity(XDocument doc)
        {
            RequestMessageBase requestMessage = null;
            RequestMsgType msgType;
            try
            {
                msgType = MessageTypeUtility.GetRequestMsgType(doc);
                switch (msgType)
                {
                    case RequestMsgType.Text:
                        requestMessage = new RequestMessageText();
                        break;
                    case RequestMsgType.Location:
                        requestMessage = new RequestMessageLocation();
                        break;
                    case RequestMsgType.Image:
                        requestMessage = new RequestMessageImage();
                        break;
                    case RequestMsgType.Voice:
                        requestMessage = new RequestMessageVoice();
                        break;
                    case RequestMsgType.Video:
                        requestMessage = new RequestMessageVideo();
                        break;
                    case RequestMsgType.Link:
                        requestMessage = new RequestMessageLink();
                        break;
                    case RequestMsgType.Event:
                        //判断Event类型
                        string sEventValue = doc.Root.Element("Event").Value;
                        WeixinEvent eventValue =  (WeixinEvent)Enum.Parse(typeof(WeixinEvent),sEventValue);
                        switch (eventValue)
                        {
                            case WeixinEvent.ENTER://进入会话
                                requestMessage = new RequestMessageEventEnter();
                                break;
                            case WeixinEvent.LOCATION://地理位置
                                requestMessage = new RequestMessageEventLocation();
                                break;
                            case WeixinEvent.subscribe://订阅（关注）
                                requestMessage = new RequestMessageEventSubscribe();
                                break;
                            case WeixinEvent.unsubscribe://取消订阅（关注）
                                requestMessage = new RequestMessageEventUnsubscribe();
                                break;
                            case WeixinEvent.CLICK://菜单点击
                                requestMessage = new RequestMessageEventClick();
                                break;
                            case WeixinEvent.scan://二维码扫描
                                requestMessage = new RequestMessageEventScan();
                                break;
                            case WeixinEvent.VIEW://URL跳转
                                requestMessage = new RequestMessageEventView();
                                break;
                            default://其他意外类型（也可以选择抛出异常）
                                throw new NotImplementedException("微信事件为[" + sEventValue + "]的事件未得到处理。请联系系统管理员，谢谢。");
                        }
                        break;
                    default:
                        throw new NotImplementedException("微信消息类型为[" + msgType.ToString() + "]的消息未得到处理。请联系系统管理员，谢谢。");
                }
                Extensions.FillEntityWithXml(requestMessage, doc);
            }
            catch (ArgumentException ex)
            {
                throw new WeixinException(string.Format("RequestMessage转换出错！请联系系统管理员，谢谢。\r\n\r\nXML：{0}", doc.ToString()), ex);
            }
            return requestMessage;
        }


        /// <summary>
        /// 获取XDocument转换后的IRequestMessageBase实例。
        /// 如果MsgType不存在，抛出UnknownRequestMsgTypeException异常
        /// </summary>
        /// <returns></returns>
        public static IRequestMessageBase GetRequestEntity(string xml)
        {
            return GetRequestEntity(XDocument.Parse(xml));
        }


        /// <summary>
        /// 获取XDocument转换后的IRequestMessageBase实例。
        /// 如果MsgType不存在，抛出UnknownRequestMsgTypeException异常
        /// </summary>
        /// <param name="stream">如Request.InputStream</param>
        /// <returns></returns>
        public static IRequestMessageBase GetRequestEntity(Stream stream)
        {
            using (XmlReader xr = XmlReader.Create(stream))
            {
                var doc = XDocument.Load(xr);
                return GetRequestEntity(doc);
            }
        }
    }
}
