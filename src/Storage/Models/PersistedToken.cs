using System.ComponentModel.DataAnnotations;

namespace Myvas.AspNetCore.Weixin.Models;

/// <summary>
/// A model for a persisted token.
/// </summary>
public class PersistedToken
{
    /// <summary>
    /// Gets or sets the record identifier.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// 微信公众号唯一标识
    /// </summary>
    public string AppId { get; set; }

    /// <summary>
    /// Gets or sets the type.
    /// </summary>
    /// <value>
    /// The type.
    /// </value>
    public string Type { get; set; } = WeixinStorageConstants.TokenTypes.AccessToken;

    /// <summary>
    /// 微信公众号全局唯一票据。存储空间需要512个字符或以上。
    /// </summary>
    /// <example>XkC7gqcx7pctuGe5zPIita23N7dJKIfkwz_2ULReV_Pn7T09lMyhTwlgGK5cghtqGQlPUlZ7ur_nMqhGuJNXwA</example>
    public string Data { get; set; }

    /// <summary>
    /// Gets or sets the creation time.
    /// </summary>
    /// <value>
    /// The creation time.
    /// </value>
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// Gets or sets the expiration.
    /// </summary>
    /// <value>
    /// The expiration.
    /// </value>
    public DateTime? ExpirationTime { get; set; }

    /// <summary>
    /// Gets or sets the consumed time.
    /// </summary>
    /// <value>
    /// The consumed time.
    /// </value>
    public DateTime? ConsumedTime { get; set; }

    /// <summary>
    /// 凭证有效时间。单位：秒。
    /// <para>例如：7200</para>
    /// </summary>
    public int ExpiresIn { get; set; } = 7200;
}
