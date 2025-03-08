using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public static class WeixinMenuJsonDeserializerForApi
{
    public static async Task<WeixinMenu> DeserializeAsync(Stream json, CancellationToken cancellationToken = default)
    {
        WeixinMenu menu = new WeixinMenu($"WeixinMenuOnSite{DateTime.Now:yyyyMMddHHmmssnn}");
        using (JsonDocument doc = await JsonDocument.ParseAsync(json, default, cancellationToken))
        {
            var root = doc.RootElement;
            bool enabled = root.GetProperty("is_menu_open").GetInt32() == 1;

            var selfmenu_infoElement = root.GetProperty("selfmenu_info");
            var buttonElement = selfmenu_infoElement.GetProperty("button");
            foreach (JsonElement buttonItemElement in buttonElement.EnumerateArray())
            {
                menu = Deserialize(menu, null, buttonItemElement);
            }
        }
        return menu;
    }

    public static WeixinMenu Deserialize(string json)
    {
        WeixinMenu menu = new WeixinMenu($"WeixinMenuOnSite{DateTime.Now:yyyyMMddHHmmssnn}");
        using (JsonDocument doc = JsonDocument.Parse(json))
        {
            var root = doc.RootElement;
            bool enabled = root.GetProperty("is_menu_open").GetInt32() == 1;

            var selfmenu_infoElement = root.GetProperty("selfmenu_info");
            var buttonElement = selfmenu_infoElement.GetProperty("button");
            foreach (JsonElement buttonItemElement in buttonElement.EnumerateArray())
            {
                menu = Deserialize(menu, null, buttonItemElement);
            }
        }
        return menu;
    }

    private static WeixinMenu Deserialize(WeixinMenu menu, WeixinMenuItem parent, JsonElement menuItemElement)
    {
        var nameValue = menuItemElement.GetProperty("name").GetString();

        if (menuItemElement.TryGetProperty("type", out JsonElement typeElement))
        {
            var typeValue = typeElement.GetString();
            WeixinMenuItem menuItem = WeixinMenuItemBuilder.Create(typeValue);
            menuItem.ParentId = parent?.Id;
            menuItem.Name = nameValue;
            if (menuItem is IWeixinMenuItemHasKey itemHasKey)
            {
                var keyValue = menuItemElement.GetProperty("key").GetString();
                itemHasKey.Key = keyValue;
            }
            if (menuItem is IWeixinMenuItemHasUrl itemHasUrl)
            {
                var urlValue = menuItemElement.GetProperty("url").GetString();
                itemHasUrl.Url = urlValue;
            }

            // add to WeixinMenuItem chains
            menu.AddItem(menuItem);
        }
        else if (menuItemElement.TryGetProperty("sub_button", out JsonElement subbuttonElement))
        {
            WeixinMenuItem menuItem = new WeixinMenuItem
            {
                ParentId = parent?.Id,
                Name = nameValue
            };
            menu.AddItem(menuItem);

            var listElement = subbuttonElement.GetProperty("list");
            foreach (var listItemElement in listElement.EnumerateArray())
            {
                menu = Deserialize(menu, menuItem, listItemElement);
            }
        }

        return menu;
    }
}
