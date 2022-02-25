﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin
{
    public interface IResponseMessageBase : IMessageBase
    {
        ResponseMsgType MsgType { get; }
    }

    /// <summary>
    /// 响应回复消息
    /// </summary>
    public class ResponseMessageBase : MessageBase, IResponseMessageBase
    {
        [XmlElement("MsgType", Namespace = "")]
        public string MsgTypeText { get; set; }

        [XmlIgnore]
        public ResponseMsgType MsgType
        {
            get=> (ResponseMsgType)Enum.Parse(typeof(ResponseMsgType), MsgTypeText);
            set => MsgTypeText = value.ToString();
        }

        /// <summary>
        /// 获取响应类型实例，并初始化
        /// </summary>
        /// <param name="requestMessage">请求</param>
        /// <param name="msgType">响应类型</param>
        /// <returns></returns>
        [Obsolete("建议使用CreateFromRequestMessage<T>(IRequestMessageBase requestMessage)取代此方法")]
        public static ResponseMessageBase CreateFromRequestMessage(IRequestMessageBase requestMessage, ResponseMsgType msgType)
        {
            ResponseMessageBase responseMessage = null;
            try
            {
                switch (msgType)
                {
                    case ResponseMsgType.text:
                        responseMessage = new ResponseMessageText();
                        break;
                    case ResponseMsgType.news:
                        responseMessage = new ResponseMessageNews();
                        break;
                    case ResponseMsgType.music:
                        responseMessage = new ResponseMessageMusic();
                        break;
                    case ResponseMsgType.image:
                        responseMessage = new ResponseMessageImage();
                        break;
                    case ResponseMsgType.voice:
                        responseMessage = new ResponseMessageVoice();
                        break;
                    case ResponseMsgType.video:
                        responseMessage = new ResponseMessageVideo();
                        break;
                    case ResponseMsgType.transfer_customer_service:
                        responseMessage = new ResponseMessageTransferCustomerService();
                        break;
                    default:
                        throw new WeixinUnknownRequestMsgTypeException(string.Format("ResponseMsgType没有为 {0} 提供对应处理程序。", msgType), new ArgumentOutOfRangeException());
                }

                responseMessage.ToUserName = requestMessage.FromUserName;
                responseMessage.FromUserName = requestMessage.ToUserName;
                responseMessage.CreateTime = DateTime.Now; //使用当前最新时间

            }
            catch (Exception ex)
            {
                throw new WeixinException("CreateFromRequestMessage过程发生异常", ex);
            }

            return responseMessage;
        }

        /// <summary>
        /// 获取响应类型实例，并初始化
        /// </summary>
        /// <typeparam name="T">需要返回的类型</typeparam>
        /// <param name="requestMessage">请求数据</param>
        /// <returns></returns>
        public static T CreateFromRequestMessage<T>(IRequestMessageBase requestMessage) where T : ResponseMessageBase
        {
            ResponseMessageBase responseMessage = null;
            try
            {
                responseMessage = Activator.CreateInstance<T>();

            }
            catch
            {
                throw new WeixinUnknownRequestMsgTypeException(string.Format("没有为 {0} 提供消息处理程序。", typeof(T).Name.Replace("ResponseMessage", "")), new ArgumentOutOfRangeException());
            }

            try
            {
                if (responseMessage != null)
                {
                    responseMessage.ToUserName = requestMessage.FromUserName;
                    responseMessage.FromUserName = requestMessage.ToUserName;
                    responseMessage.CreateTime = DateTime.Now; //使用当前最新时间
                }
                return responseMessage as T;
            }
            catch (Exception ex)
            {
                throw new WeixinException("ResponseMessageBase.CreateFromRequestMessage<T>过程发生异常！", ex);
            }
        }

        /// <summary>
        /// 从返回结果XML转换成IResponseMessageBase实体类
        /// </summary>
        /// <param name="xml">返回给服务器的Response Xml</param>
        /// <returns></returns>
        public static IResponseMessageBase CreateFromResponseXml(string xml)
        {
            try
            {
                if (string.IsNullOrEmpty(xml))
                {
                    return null;
                }

                var doc = XDocument.Parse(xml);
                ResponseMessageBase responseMessage = null;
                string sMsgType = doc.Root.Element("MsgType").Value;
                var msgType = (ResponseMsgType)Enum.Parse(typeof(ResponseMsgType), sMsgType, true);
                switch (msgType)
                {
                    case ResponseMsgType.text:
                        responseMessage = new ResponseMessageText();
                        break;
                    case ResponseMsgType.image:
                        responseMessage = new ResponseMessageImage();
                        break;
                    case ResponseMsgType.voice:
                        responseMessage = new ResponseMessageVoice();
                        break;
                    case ResponseMsgType.video:
                        responseMessage = new ResponseMessageVideo();
                        break;
                    case ResponseMsgType.music:
                        responseMessage = new ResponseMessageMusic();
                        break;
                    case ResponseMsgType.news:
                        responseMessage = new ResponseMessageNews();
                        break;
                    default:
                        throw new NotImplementedException("没有实现该消息类型:" + doc.Root.Element("MsgType").Value);
                }

                responseMessage.FillEntityWithXml(doc);
                return responseMessage;
            }
            catch (Exception ex)
            {
                throw new WeixinException("ResponseMessageBase.CreateFromResponseXml<T>过程发生异常！" + ex.Message, ex);
            }
        }
    }
}
