using System.ComponentModel.DataAnnotations;

namespace Myvas.AspNetCore.Weixin.Models;

/// <summary>
/// A model for a persisted token.
/// </summary>
public class PersistedToken
{
    /// <summary>
    /// Gets or sets the WeixinApp's identifier.
    /// </summary>
    public string AppId { get; set; }

    /// <summary>
    /// 微信公众号全局唯一票据。存储空间需要512个字符或以上。
    /// </summary>
    /// <example>XkC7gqcx7pctuGe5zPIita23N7dJKIfkwz_2ULReV_Pn7T09lMyhTwlgGK5cghtqGQlPUlZ7ur_nMqhGuJNXwA</example>
    public string Token { get; set; }

    /// <summary>
    /// Gets or sets the expiration time.
    /// </summary>
    /// <value>
    /// The expiration.
    /// </value>
    public DateTime ExpirationTime { get; set; }
}
