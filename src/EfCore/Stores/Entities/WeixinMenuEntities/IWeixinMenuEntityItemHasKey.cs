﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin.Entities;

public interface IWeixinMenuEntityItemHasKey
{
    [JsonPropertyName("key")]
    [MaxLength(128, ErrorMessage = "菜单KEY值不能超过128个字节")]
    string Key { get; set; }
}
