﻿using Myvas.AspNetCore.Weixin.Site.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Myvas.AspNetCore.Weixin
{
    public static class ResponseMessageFactory
    {
        //<?xml version="1.0" encoding="utf-8"?>
        //<xml>
        //  <ToUserName><![CDATA[olPjZjsXuQPJoV0HlruZkNzKc91E]]></ToUserName>
        //  <FromUserName><![CDATA[gh_a96a4a619366]]></FromUserName>
        //  <CreateTime>63497820384</CreateTime>
        //  <MsgType>text</MsgType>
        //  <Content><![CDATA[您刚才发送了文字信息：中文
        //您还可以发送【位置】【图片】【语音】信息，查看不同格式的回复。
        //SDK官方地址：http://...]]></Content>
        //  <FuncFlag>0</FuncFlag>
        //</xml>

        /// <summary>
        /// 获取XDocument转换后的IResponseMessageBase实例（通常在反向读取日志的时候用到）。
        /// 如果MsgType不存在，抛出UnknownRequestMsgTypeException异常
        /// </summary>
        /// <returns></returns>
        public static IResponseMessageBase GetResponseEntity(XDocument doc)
        {
            ResponseMessageBase responseMessage = null;
            ResponseMsgType msgType;
            try
            {
                msgType = MsgTypeHelper.GetResponseMsgType(doc);
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
                    //case ResponseMsgType.Transfer_Customer_Service:
                    //    responseMessage = new ResponseMessageTransfer_Customer_Service();
                    //    break;
                    default:
                        throw new WeixinUnknownRequestMsgTypeException(string.Format("MsgType：{0} 在ResponseMessageFactory中没有对应的处理程序！", msgType), new ArgumentOutOfRangeException());
                }
                MessageBaseXmlExtensions.FillEntityWithXml(responseMessage, doc);
            }
            catch (ArgumentException ex)
            {
                throw new WeixinException(string.Format("ResponseMessage转换出错！可能是MsgType不存在！，XML：{0}", doc.ToString()), ex);
            }
            return responseMessage;
        }


        /// <summary>
        /// 获取XDocument转换后的IRequestMessageBase实例。
        /// 如果MsgType不存在，抛出UnknownRequestMsgTypeException异常
        /// </summary>
        /// <returns></returns>
        public static IResponseMessageBase GetResponseEntity(string xml)
        {
            return GetResponseEntity(XDocument.Parse(xml));
        }
    }
}
