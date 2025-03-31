using System;
using System.Linq;

namespace Myvas.AspNetCore.Weixin;

public static class WeixinMenuCreateJsonExtensions
{
    /// <summary>
    /// Add a button to this menu.
    /// </summary>
    /// <param name="me"></param>
    /// <param name="menuItem"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static WeixinMenuCreateJson AddButton(this WeixinMenuCreateJson me, WeixinMenuJson.Button menuItem)
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

    /// <summary>
    /// Add a button under a specific container (also button).
    /// </summary>
    /// <param name="me"></param>
    /// <param name="parentButtonName"></param>
    /// <param name="menuItem"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static WeixinMenuCreateJson AddSubButton(this WeixinMenuCreateJson me, string parentButtonName, WeixinMenuJson.Button menuItem)
    {
        if (me == null) throw new ArgumentNullException(nameof(me));

        if (string.IsNullOrEmpty(parentButtonName))
        {
            throw new ArgumentNullException(nameof(parentButtonName));
        }
        if (menuItem == null)
        {
            throw new ArgumentNullException(nameof(menuItem));
        }
        if (string.IsNullOrEmpty(menuItem.Name))
        {
            throw new ArgumentNullException(nameof(menuItem.Name));
        }

        var parent = me.Buttons
            .OfType<WeixinMenuJson.Button.Container>()
            .FirstOrDefault(x => x.Name == parentButtonName);
        if (parent == null)
        {
            throw new ArgumentException($"The parent button '{parentButtonName}' does not exist.");
        }

        parent.Buttons.Add(menuItem);
        return me;
    }
}
