using System.Linq;

namespace Myvas.AspNetCore.Weixin;

public static class WeixinCurrentMenuVersionExtensions
{
    /// <summary>
    /// Check if has a button of type "click" in <see cref="WeixinCurrentMenuJson"/>.
    /// </summary>
    /// <param name="me"></param>
    /// <returns></returns>
    public static bool HasButtonClick(this WeixinCurrentMenuJson me)
    {
        if (me?.SelfMenuInfo?.Buttons?.OfType<WeixinMenuJson.Button.Click>()?.Any() ?? false)
        {
            return true;
        }
        foreach (var container in (me?.SelfMenuInfo?.Buttons?.OfType<WeixinMenuJson.Button.ContainerWithList>() ?? []))
        {
            if (container.HasButtonClick()) return true;
        }
        return false;
    }

    /// <summary>
    /// Check if has a button of type "click" in <see cref="WeixinMenuJson.Button.ContainerWithList" />.
    /// </summary>
    /// <param name="me"></param>
    /// <returns></returns>
    public static bool HasButtonClick(this WeixinMenuJson.Button.ContainerWithList me)
    {
        return (me?.SubButton?.Buttons?.OfType<WeixinMenuJson.Button.Click>()?.Any() ?? false);
    }
}
