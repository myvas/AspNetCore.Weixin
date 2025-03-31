using System;
using System.Linq;

namespace Myvas.AspNetCore.Weixin;

public static class WeixinMenuJsonButtonContainerWithListxtensions
{
    /// <summary>
    /// Add a button to this container (also button).
    /// </summary>
    /// <param name="me"></param>
    /// <param name="menuItem"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static WeixinMenuJson.Button.ContainerWithList AddButton(this WeixinMenuJson.Button.ContainerWithList me, WeixinMenuJson.Button menuItem)
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

        if (me.SubButton.Buttons.Any(x => x.Name == menuItem.Name))
        {
            throw new ArgumentException($"A button '{menuItem.Name}' already exists.");
        }

        me.SubButton.Buttons.Add(menuItem);
        return me;
    }
}
