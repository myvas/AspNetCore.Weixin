﻿namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 上传媒体文件返回结果
/// </summary>
public class UploadMediaResult : WeixinErrorJson
{
    public MediaType UploadMediaType { get; set; }
    public string media_id { get; set; }
    public long created_at { get; set; }
}
