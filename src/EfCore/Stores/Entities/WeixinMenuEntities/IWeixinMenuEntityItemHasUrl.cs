﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin.Entities;

public interface IWeixinMenuEntityItemHasUrl
{
    [JsonPropertyName("url")]
    [MaxLength(1024, ErrorMessage = "网页链接不能超过1024个字节")]
    string Url { get; set; }
}
