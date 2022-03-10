using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin;

public static class WeixinXmlConvert
{
    /// <summary>
    /// 序列化
    /// </summary>
    /// <param name="objectInstance"></param>
    /// <param name="encoding">编码，默认为：System.Text.Encoding.UTF8</param>
    /// <returns></returns>
    public static string SerializeObject(object objectInstance, bool omitAllXsiXsd = true, Encoding encoding = null)
    {
        if (encoding == null) encoding = Encoding.UTF8;

        var serializer = new XmlSerializer(objectInstance.GetType());
        var settings = new XmlWriterSettings
        {
            OmitXmlDeclaration = omitAllXsiXsd,
            Indent = true,
            IndentChars = "    ",
            NewLineChars = Environment.NewLine,
            Encoding = encoding
        };

        //var sb = new StringBuilder();//The default encoding of StringBuilder is UTF16, so we have to change it.
        var sb = new StringWriterWithEncoding(encoding);

        using (var writer = XmlWriter.Create(sb, settings))
        {
            if (omitAllXsiXsd)
            {
                var ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                serializer.Serialize(writer, objectInstance, ns);
            }
            else
            {
                serializer.Serialize(writer, objectInstance);
            }
        }

        return sb.ToString();
    }

    /// <summary>
    /// 反序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="objectData"></param>
    /// <returns></returns>
    public static T DeserializeObject<T>(string objectData)
    {
        return (T)DeserializeObject(objectData, typeof(T));
    }

    private static object DeserializeObject(string objectData, Type type)
    {
        var serializer = new XmlSerializer(type);
        object result;

        using (TextReader reader = new StringReader(objectData))
        {
            result = serializer.Deserialize(reader);
        }

        return result;
    }

    internal sealed class StringWriterWithEncoding : StringWriter
    {
        private readonly Encoding encoding;

        public StringWriterWithEncoding(Encoding encoding)
        {
            this.encoding = encoding;
        }

        public override Encoding Encoding
        {
            get { return encoding; }
        }
    }
}
