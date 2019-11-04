using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Myvas.AspNetCore.Weixin
{

    internal static class Extensions
    {
        /// <summary>
        /// 根据XML信息填充实实体
        /// </summary>
        /// <typeparam name="T">MessageBase为基类的类型，Response和Request都可以</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="doc">XML</param>
        public static void FillEntityWithXml<T>(this T entity, XDocument doc) where T : /*MessageBase*/ class, new()
        {
            entity = entity ?? new T();
            var root = doc.Root;

            var props = entity.GetType().GetProperties();
            foreach (var prop in props)
            {
                var propName = prop.Name;
                if (root.Element(propName) != null)
                {
                    if (prop.PropertyType == typeof(DateTime))
                    {
                        prop.SetValue(entity, WeixinTimestampHelper.ToLocalTime(root.Element(propName).Value), null);
                    }
                    else if (prop.PropertyType == typeof(Boolean) && (propName == "FuncFlag"))
                    {
                        prop.SetValue(entity, root.Element(propName).Value == "1", null);
                    }
                    else if (prop.PropertyType == typeof(Int32))
                    {
                        prop.SetValue(entity, int.Parse(root.Element(propName).Value), null);
                    }
                    else if (prop.PropertyType == typeof(Int64))
                    {

                        prop.SetValue(entity, long.Parse(root.Element(propName).Value), null);
                    }
                    else if (prop.PropertyType == typeof(Decimal))
                    {
                        prop.SetValue(entity, decimal.Parse(root.Element(propName).Value), null);
                    }
                    else if (prop.PropertyType == typeof(Double))
                    {
                        prop.SetValue(entity, double.Parse(root.Element(propName).Value), null);
                    }
                    else if (prop.PropertyType == typeof(ReceivedMsgType))
                    {
                        //已设为只读
                        //prop.SetValue(entity, MsgTypeHelper.GetRequestMsgType(root.Element(propName).Value), null);
                    }
                    else if (prop.PropertyType == typeof(ResponseMsgType))
                    {
                        //已设为只读
                        //prop.SetValue(entity, MsgTypeHelper.GetResponseMsgType(root.Element(propName).Value), null);
                    }
                    else if (prop.PropertyType == typeof(ReceivedEventType))
                    {
                        //已设为只读
                        //prop.SetValue(entity, EventHelper.GetEventType(root.Element(propName).Value), null);
                    }
                    else if (prop.PropertyType == typeof(List<Article>))
                    {
                        //var genericArguments = prop.PropertyType.GetGenericArguments();
                        //if (genericArguments[0].Name == "Article")//ResponseMessageNews适用
                        {
                            //文章下属节点item
                            List<Article> articles = new List<Article>();
                            foreach (var item in root.Element(propName).Elements("item"))
                            {
                                var article = new Article();
                                FillEntityWithXml(article, new XDocument(item));
                                articles.Add(article);
                            }
                            prop.SetValue(entity, articles, null);
                        }
                    }
                    else if (prop.PropertyType == typeof(Music))
                    {
                        Music music = new Music();
                        FillEntityWithXml(music, new XDocument(root.Element(propName)));
                        prop.SetValue(entity, music, null);
                    }
                    else if (prop.PropertyType == typeof(Image))
                    {
                        Image image = new Image();
                        FillEntityWithXml(image, new XDocument(root.Element(propName)));
                        prop.SetValue(entity, image, null);
                    }
                    else if (prop.PropertyType == typeof(Voice))
                    {
                        Voice voice = new Voice();
                        FillEntityWithXml(voice, new XDocument(root.Element(propName)));
                        prop.SetValue(entity, voice, null);
                    }
                    else if (prop.PropertyType == typeof(Video))
                    {
                        Video video = new Video();
                        FillEntityWithXml(video, new XDocument(root.Element(propName)));
                        prop.SetValue(entity, video, null);
                    }
                    else
                    {
                        prop.SetValue(entity, root.Element(propName).Value, null);
                    }
                }
            }
        }

        /// <summary>
        /// 将实体转为XML
        /// </summary>
        /// <typeparam name="T">RequestMessage或ResponseMessage</typeparam>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public static XDocument ConvertEntityToXml<T>(this T entity) where T : class , new()
        {
            entity = entity ?? new T();
            var doc = new XDocument();
            doc.Add(new XElement("xml"));
            var root = doc.Root;

            /* 注意！
             * 经过测试，微信对字段排序有严格要求，这里对排序进行强制约束
            */
            var propNameOrder = new List<string>() { "ToUserName", "FromUserName", "CreateTime", "MsgType" };
            //不同返回类型需要对应不同特殊格式的排序
            if (entity is ResponseMessageNews)
            {
                propNameOrder.AddRange(new[] { "ArticleCount", "Articles", "FuncFlag",/*以下是Atricle属性*/ "Title ", "Description ", "PicUrl", "Url" });
            }
            else if (entity is ResponseMessageMusic)
            {
                propNameOrder.AddRange(new[] { "Music", "FuncFlag", "ThumbMediaId",/*以下是Music属性*/ "Title ", "Description ", "MusicUrl", "HQMusicUrl" });
            }
            else if (entity is ResponseMessageImage)
            {
                propNameOrder.AddRange(new[] { "Image",/*以下是Image属性*/ "MediaId " });
            }
            else if (entity is ResponseMessageVoice)
            {
                propNameOrder.AddRange(new[] { "Voice",/*以下是Voice属性*/ "MediaId " });
            }
            else if (entity is ResponseMessageVideo)
            {
                propNameOrder.AddRange(new[] { "Video",/*以下是Video属性*/ "MediaId ", "Title", "Description" });
            }
            else
            {
                //如Text类型
                propNameOrder.AddRange(new[] { "Content", "FuncFlag" });
            }

            Func<string, int> orderByPropName = propNameOrder.IndexOf;

            var props = entity.GetType().GetProperties().OrderBy(p => orderByPropName(p.Name)).ToList();
            foreach (var prop in props)
            {
                var propName = prop.Name;
                if (propName == "Articles")
                {
                    //文章列表
                    var atriclesElement = new XElement("Articles");
                    var articales = prop.GetValue(entity, null) as List<Article>;
                    foreach (var articale in articales)
                    {
                        var subNodes = ConvertEntityToXml(articale).Root.Elements();
                        atriclesElement.Add(new XElement("item", subNodes));
                    }
                    root.Add(atriclesElement);
                }
                else if (propName == "Music" || propName == "Image" || propName == "Video" || propName == "Voice")
                {
                    //音乐、图片、视频、语音格式
                    var musicElement = new XElement(propName);
                    var media = prop.GetValue(entity, null);// as Music;
                    var subNodes = ConvertEntityToXml(media).Root.Elements();
                    musicElement.Add(subNodes);
                    root.Add(musicElement);
                }
                else
                {
                    switch (prop.PropertyType.Name)
                    {
                        case "String":
                            root.Add(new XElement(propName,
                                                  new XCData(prop.GetValue(entity, null) as string ?? "")));
                            break;
                        case "DateTime":
                            root.Add(new XElement(propName, WeixinTimestampHelper.FromBeijingTime((DateTime)prop.GetValue(entity, null))));
                            break;
                        case "Boolean":
                            if (propName == "FuncFlag")
                            {
                                root.Add(new XElement(propName, (bool)prop.GetValue(entity, null) ? "1" : "0"));
                            }
                            else
                            {
                                goto default;
                            }
                            break;
                        case "ResponseMsgType":
                            root.Add(new XElement(propName, new XCData(prop.GetValue(entity, null).ToString().ToLower())));
                            break;
                        case "Article":
                            root.Add(new XElement(propName, prop.GetValue(entity, null).ToString().ToLower()));
                            break;
                        default:
                            root.Add(new XElement(propName, prop.GetValue(entity, null)));
                            break;
                    }
                }
            }
            return doc;
        }

        /// <summary>
        /// 将实体转为XML字符串
        /// </summary>
        /// <typeparam name="T">RequestMessage或ResponseMessage</typeparam>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public static string ConvertEntityToXmlString<T>(this T entity) where T : class , new()
        {
            return entity.ConvertEntityToXml().ToString();
        }

        /// <summary>
        /// ResponseMessageBase.CreateFromRequestMessage<T>(requestMessage)的扩展方法
        /// </summary>
        /// <typeparam name="T">需要生成的ResponseMessage类型</typeparam>
        /// <param name="requestMessage">IRequestMessageBase接口下的接收信息类型</param>
        /// <returns></returns>
        public static T CreateResponseMessage<T>(this IRequestMessageBase requestMessage) where T : ResponseMessageBase
        {
            return ResponseMessageBase.CreateFromRequestMessage<T>(requestMessage);
        }

        /// <summary>
        /// ResponseMessageBase.CreateFromResponseXml(xml)的扩展方法
        /// </summary>
        /// <param name="xml">返回给服务器的Response Xml</param>
        /// <returns></returns>
        public static IResponseMessageBase CreateResponseMessage(this string xml)
        {
            return ResponseMessageBase.CreateFromResponseXml(xml);
        }
    }

}
