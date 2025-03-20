using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 微信语言代码。代码规则同国际通用标准。
/// </summary>
public class WeixinLanguage
{
    private static readonly Dictionary<string, string> _table = new()
    {
        {"en", "English"},
        {"zh_CN", "中文简体"},
        {"zh_TW", "中文繁體"}
    };

    public string Code { get; private set; }
    public string DisplayName { get; private set; }

    private WeixinLanguage(string code, string displayName)
    {
        (Code, DisplayName) = (code, displayName);
    }

    public WeixinLanguage(string code)
    {
        Code = code ?? throw new ArgumentNullException(nameof(code));
        DisplayName = _table.GetValueOrDefault(code) ?? throw new ArgumentNullException(nameof(code));
    }

    public override string ToString()
    {
        return Code;
    }

    public static WeixinLanguage FromDisplayName(string displayName)
    {
        if (displayName == null) throw new ArgumentNullException(nameof(displayName));
        var item = _table.FirstOrDefault(x => x.Value == displayName);
        return new WeixinLanguage(item.Key, item.Value);

    }

    /// <summary>
    /// 中文简体
    /// </summary>
    public static readonly WeixinLanguage zh_CN = new("zh_CN", "中文简体");

    /// <summary>
    /// 中文繁體
    /// </summary>
    public static readonly WeixinLanguage zh_TW = new("zh_TW", "中文繁體");

    /// <summary>
    /// English
    /// </summary>
    public static readonly WeixinLanguage en = new("en", "English");
}
