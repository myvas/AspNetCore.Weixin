using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin;

public class WeixinMenuJson_ButtonConverter : JsonConverter<WeixinMenuJson.Button>
{
    public override WeixinMenuJson.Button Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        using var jsonDoc = JsonDocument.ParseValue(ref reader);
        var root = jsonDoc.RootElement;

        if (root.TryGetProperty("type", out var typeProp))
        {
            var type = typeProp.GetString();
            return type switch
            {
                "click" => JsonSerializer.Deserialize<WeixinMenuJson.Button.Click>(root.GetRawText(), options)!,
                "view" => JsonSerializer.Deserialize<WeixinMenuJson.Button.View>(root.GetRawText(), options)!,
                "text" => JsonSerializer.Deserialize<WeixinMenuJson.Button.Text>(root.GetRawText(), options)!,
                "img" => JsonSerializer.Deserialize<WeixinMenuJson.Button.Image>(root.GetRawText(), options)!,
                "video" => JsonSerializer.Deserialize<WeixinMenuJson.Button.Video>(root.GetRawText(), options)!,
                "voice" => JsonSerializer.Deserialize<WeixinMenuJson.Button.Voice>(root.GetRawText(), options)!,
                "news" => JsonSerializer.Deserialize<WeixinMenuJson.Button.News>(root.GetRawText(), options)!,
                "miniprogram" => JsonSerializer.Deserialize<WeixinMenuJson.Button.ViewMiniprogram>(root.GetRawText(), options)!,
                "scancode_push" => JsonSerializer.Deserialize<WeixinMenuJson.Button.ScanQrcode>(root.GetRawText(), options)!,
                "scancode_waitmsg" => JsonSerializer.Deserialize<WeixinMenuJson.Button.ScanQrcodeAndWait>(root.GetRawText(), options)!,
                "pic_sysphoto" => JsonSerializer.Deserialize<WeixinMenuJson.Button.TakePhoto>(root.GetRawText(), options)!,
                "pic_photo_or_album" => JsonSerializer.Deserialize<WeixinMenuJson.Button.TakePhotoOrChoose>(root.GetRawText(), options)!,
                "pic_weixin" => JsonSerializer.Deserialize<WeixinMenuJson.Button.ChooseFromWeixinAlbum>(root.GetRawText(), options)!,
                "location_select" => JsonSerializer.Deserialize<WeixinMenuJson.Button.ReportLocation>(root.GetRawText(), options)!,
                "media_id" => JsonSerializer.Deserialize<WeixinMenuJson.Button.DownloadMedia>(root.GetRawText(), options)!,
                "view_limited" => JsonSerializer.Deserialize<WeixinMenuJson.Button.ViewArticleByMediaId>(root.GetRawText(), options)!,
                "article_id" => JsonSerializer.Deserialize<WeixinMenuJson.Button.DownloadArticleCard>(root.GetRawText(), options)!,
                "article_view_limited" => JsonSerializer.Deserialize<WeixinMenuJson.Button.ViewArticleByArticleId>(root.GetRawText(), options)!,
                _ => throw new JsonException($"Unknown button type: {type}")
            };
        }
        else if (root.TryGetProperty("sub_button", out var containerProp))
        {
            return containerProp.TryGetProperty("list", out _)
                ? JsonSerializer.Deserialize<WeixinMenuJson.Button.ContainerWithList>(root.GetRawText(), options)!
                : JsonSerializer.Deserialize<WeixinMenuJson.Button.Container>(root.GetRawText(), options)!;
        }
        else
        {
            throw new JsonException("Invalid button structure");
        }
    }

    public override void Write(
        Utf8JsonWriter writer,
        WeixinMenuJson.Button value,
        JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, (object)value, options);
    }
}