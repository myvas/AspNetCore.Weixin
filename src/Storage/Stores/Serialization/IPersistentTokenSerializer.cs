using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin.Stores.Serialization;

/// <summary>
/// Interface for persisted token serialization
/// </summary>
public interface IPersistentTokenSerializer
{
    /// <summary>
    /// Serializes the specified value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    string Serialize<T>(T value);

    /// <summary>
    /// Deserializes the specified string.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json">The json.</param>
    /// <returns></returns>
    T Deserialize<T>(string json);
}

/// <summary>
/// Options for how persisted grants are persisted.
/// </summary>
public class PersistentGrantOptions
{
    /// <summary>
    /// Data protect the persisted grants "data" column.
    /// </summary>
    public bool DataProtectData { get; set; } = true;
}
