using System;
using System.Linq;

namespace Myvas.AspNetCore.Weixin;

public static class WeixinMenuJsonButtonContainerExtensions
{
    /// <summary>
    /// Add a button to this container (also button).
    /// </summary>
    /// <param name="me"></param>
    /// <param name="menuItem"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static WeixinMenuJson.Button.Container AddButton(this WeixinMenuJson.Button.Container me, WeixinMenuJson.Button menuItem)
    {
        if (me == null) throw new ArgumentNullException(nameof(me));

        if (menuItem == null)
        {
            throw new ArgumentNullException(nameof(menuItem));
        }
        if (string.IsNullOrEmpty(menuItem.Name))
        {
            throw new ArgumentNullException(nameof(menuItem.Name));
        }

        if (me.Buttons.Any(x => x.Name == menuItem.Name))
        {
            throw new ArgumentException($"A button '{menuItem.Name}' already exists.");
        }

        me.Buttons.Add(menuItem);
        return me;
    }
}
