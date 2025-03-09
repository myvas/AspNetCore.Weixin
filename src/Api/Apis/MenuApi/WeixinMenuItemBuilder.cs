namespace Myvas.AspNetCore.Weixin;

public static class WeixinMenuItemBuilder
{
    public static WeixinMenuItem Create(string type)
    {
        if (type == null) type = "";
        return type switch
        {
            WeixinMenuItemTypes.Click => new WeixinMenuItemClick(),
            WeixinMenuItemTypes.View => new WeixinMenuItemView(),
            WeixinMenuItemTypes.ScanPush => new WeixinMenuItemScanPush(),
            WeixinMenuItemTypes.ScanWait => new WeixinMenuItemScanWait(),
            WeixinMenuItemTypes.PicFromCameraOnly => new WeixinMenuItemPicFromCameraOnly(),
            WeixinMenuItemTypes.PicFromAlbumOrCamera => new WeixinMenuItemPicFromAlbumOrCamera(),
            WeixinMenuItemTypes.PicFromWeixinAlbum => new WeixinMenuItemPicFromWeixinAlbum(),
            WeixinMenuItemTypes.LocationSelect => new WeixinMenuItemLocationSelect(),
            WeixinMenuItemTypes.Media => new WeixinMenuItemMedia(),
            WeixinMenuItemTypes.ViewLimited => new WeixinMenuItemViewLimited(),
            WeixinMenuItemTypes.Miniprogram => new WeixinMenuItemMiniprogram(),
            _ => new WeixinMenuItem()
        };
    }
}
