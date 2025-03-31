using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin;

public partial class WeixinMenuJson
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso href=""></seealso>
#if false //NET7_0_OR_GREATER
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
    [JsonDerivedType(typeof(Button.ContainerWithList), typeDiscriminator: null)]
    [JsonDerivedType(typeof(Button.Container), typeDiscriminator: null)]
    [JsonDerivedType(typeof(Button.Click), typeDiscriminator: "click")]
    [JsonDerivedType(typeof(Button.View), typeDiscriminator: "view")]
    [JsonDerivedType(typeof(Button.Text), typeDiscriminator: "text")]
    [JsonDerivedType(typeof(Button.Image), typeDiscriminator: "img")]
    [JsonDerivedType(typeof(Button.Video), typeDiscriminator: "video")]
    [JsonDerivedType(typeof(Button.Voice), typeDiscriminator: "voice")]
    [JsonDerivedType(typeof(Button.News), typeDiscriminator: "news")]
    [JsonDerivedType(typeof(Button.ScanQrcode), typeDiscriminator: "scancode_push")]
    [JsonDerivedType(typeof(Button.ScanQrcodeAndWait), typeDiscriminator: "scancode_waitmsg")]
    [JsonDerivedType(typeof(Button.TakePhoto), typeDiscriminator: "pic_sysphoto")]
    [JsonDerivedType(typeof(Button.TakePhotoOrChoose), typeDiscriminator: "pic_photo_or_album")]
    [JsonDerivedType(typeof(Button.ChooseFromWeixinAlbum), typeDiscriminator: "pic_weixin")]
    [JsonDerivedType(typeof(Button.ReportLocation), typeDiscriminator: "location_select")]
    [JsonDerivedType(typeof(Button.DownloadMedia), typeDiscriminator: "media_id")]
    [JsonDerivedType(typeof(Button.ViewArticleByMediaId), typeDiscriminator: "view_limited")]
    [JsonDerivedType(typeof(Button.DownloadArticleCard), typeDiscriminator: "article_id")]
    [JsonDerivedType(typeof(Button.ViewArticleByArticleId), typeDiscriminator: "article_view_limited")]
#else
    [JsonConverter(typeof(WeixinMenuJson_ButtonConverter))]
#endif
    public partial class Button
    {
        public Button() { }
        public Button(string name)
        {
            Name = name;
        }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}