namespace Myvas.AspNetCore.Weixin
{

    public static class WeixinMenuItemTypes
    {
        public const string Click = "click";
        public const string View = "view";
        public const string ScanPush = "scancode_push";
        public const string ScanWait = "scancode_waitmsg";
        public const string PicFromCameraOnly = "pic_sysphoto";
        public const string PicFromAlbumOrCamera = "pic_photo_or_album";
        public const string PicFromWeixinAlbum = "pic_weixin";
        public const string LocationSelect = "location_select";
        public const string Media = "media_id";
        public const string ViewLimited = "view_limited";
        public const string Miniprogram = "miniprogram";
    }
}
