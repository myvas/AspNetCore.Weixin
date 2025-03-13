using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin;

public static class MyvasXmlConvert
{
    private static string JsonToXml(string json, string rootElementName = "xml")
    {
        using var jsonReader = JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(json), XmlDictionaryReaderQuotas.Max);
        var xdoc = XDocument.Load(jsonReader);

        // Change root element name
        xdoc.Root.Name = rootElementName;

        // Iterate through all elements and remove their attributes
        foreach (var element in xdoc.Descendants())
        {
            element.Attributes().Remove();
        }

        // Remove crlf
        return xdoc.ToString(SaveOptions.DisableFormatting);
    }

    /// <summary>
    /// 序列化：支持匿名对象
    /// </summary>
    /// <param name="objectInstance">普通类对象，或匿名对象</param>
    /// <param name="encoding">编码，默认为：System.Text.Encoding.UTF8</param>
    /// <param name="rootElementName">若不为空，则生成的XML以该字符串作为根节点，默认为"xml"。注意：XML只能有一个根节点，所以如果对象是数组，则根节点不能为空。</param>
    /// <returns></returns>
    /// <seealso cref="BaseClassFirstConverter<typeparam name="T">" />
    public static string SerializeObject<T>(T objectInstance, bool omitAllXsiXsd = true, Encoding encoding = null, string rootElementName = "xml")
        where T : class
    {
        if (objectInstance == null)
        {
            throw new ArgumentNullException(nameof(objectInstance), "The object instance to serialize cannot be null.");
        }
        if (encoding == null) encoding = Encoding.UTF8;

        var jsonText = JsonSerializer.Serialize(objectInstance, new JsonSerializerOptions
        {
            Converters = { new BaseClassFirstConverter<T>() }
        });
        var xmldoc = JsonToXml(jsonText, rootElementName);
        return xmldoc;
    }

    /// <summary>
    /// 序列化：不支持匿名对象
    /// </summary>
    /// <param name="objectInstance">普通类对象</param>
    /// <param name="encoding">编码，默认为：System.Text.Encoding.UTF8</param>
    /// <returns></returns>
    public static string SerializeTypedObject(object objectInstance, bool omitAllXsiXsd = true, Encoding encoding = null)
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

        try
        {
            //using var sb = new StringBuilder();//StringBuilder的encoding为UTF16
            using var sb = new StringWriterWithEncoding(encoding);

            using (XmlWriter writer = XmlWriter.Create(sb, settings))
            {
                if (omitAllXsiXsd)
                {
                    XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
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
        catch (Exception ex)
        {
            // Log the exception details
            Console.WriteLine($"An error occurred during serialization: {ex.Message}");
            throw;
        }
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
        private readonly Encoding _encoding;

        public StringWriterWithEncoding(Encoding encoding)
        {
            _encoding = encoding ?? Encoding.UTF8;
        }

        public override Encoding Encoding => _encoding;
    }
}
