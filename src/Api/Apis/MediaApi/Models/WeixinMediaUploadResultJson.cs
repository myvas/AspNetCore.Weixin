namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 上传媒体文件返回结果
/// </summary>
public class WeixinMediaUploadResultJson : WeixinErrorJson
{
    public WeixinUploadMediaType type;
    public string media_id;
    public long created_at;
}
