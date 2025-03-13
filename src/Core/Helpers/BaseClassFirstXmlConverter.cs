using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// A <see cref="JsonConverter"/> which use <see cref="XmlElement"/> as name, and explicitly serializes the derived class properties after the base class properties.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <seealso cref="JsonConverter"/>
/// <seealso cref="XmlElementAttribute"/>
/// <seealso cref="XmlIgnoreAttribute"/> 
public class BaseClassFirstXmlConverter<T> : JsonConverter<T>
    where T : class
{
    /// <inheritdoc />
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Implement deserialization logic if needed
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        if (value == null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();

        // Get the type of the object
        Type objectType = value.GetType();

        // Check if the object is an anonymous type
        bool isAnonymousType = objectType.Name.Contains("AnonymousType");

        if (isAnonymousType || objectType == typeof(object))
        {
            // For anonymous types or obejcts of type 'object', serialize all properties in order as it is
            var properties = objectType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                //.OrderBy(p => p.Name)
                ;

            foreach (var property in properties)
            {
                if (property.CanRead && !(options.IgnoreReadOnlyProperties && !property.CanWrite))
                {
                    var propertyValue = property.GetValue(value);
                    if (!(options.IgnoreNullValues && propertyValue == null))
                    {
                        if (property.GetCustomAttribute<XmlIgnoreAttribute>() == null)
                        {
                            var xmlElementName = property.GetCustomAttribute<XmlElementAttribute>()?.ElementName;
                            var propertyName = xmlElementName ?? property.Name;
                            writer.WritePropertyName(propertyName);
                            JsonSerializer.Serialize(writer, propertyValue, options);
                        }
                    }
                }
            }
        }
        else
        {
            // For regular objects, serialize base class properties first
            var typesInHierarchy = GetTypeHierarchy(objectType);

            foreach (var type in typesInHierarchy)
            {
                var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                foreach (var property in properties)
                {
                    if (property.CanRead && !(options.IgnoreReadOnlyProperties && !property.CanWrite))
                    {
                        var propertyValue = property.GetValue(value);
                        if (!(options.IgnoreNullValues && propertyValue == null))
                        {
                            if (property.GetCustomAttribute<XmlIgnoreAttribute>() == null)
                            {
                                var xmlElementName = property.GetCustomAttribute<XmlElementAttribute>()?.ElementName;
                                var propertyName = xmlElementName ?? property.Name;
                                writer.WritePropertyName(propertyName);
                                JsonSerializer.Serialize(writer, propertyValue, options);
                            }
                        }
                    }
                }
            }
        }

        writer.WriteEndObject();
    }

    /// <summary>
    /// Helper method to get the type hierarchy (from topmost base class to most derived class)
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private static IEnumerable<Type> GetTypeHierarchy(Type type)
    {
        var hierarchy = new List<Type>();

        while (type != null && type != typeof(object))
        {
            hierarchy.Insert(0, type); // Insert at the beginning to maintain order
            type = type.BaseType;
        }

        return hierarchy;
    }
}
