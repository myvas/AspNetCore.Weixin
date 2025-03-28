﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin.Entities;

public class WeixinMenuEntityItem
{
    #region 存储/显示
    public WeixinMenuEntityItem()
    {
        Id = Guid.NewGuid().ToString();
    }
    [JsonIgnore]
    public string Id { get; set; }
    /// <summary>
    /// 上级菜单，顶级菜单为null
    /// </summary>
    [JsonIgnore]
    public string ParentId { get; set; } = null;
    /// <summary>
    /// 序号。特别地，有多个同级菜单序号相同时，显示顺序不确定。
    /// </summary>
    [JsonIgnore]
    public int Order { get; set; }
    #endregion

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("sub_button")]
    public IList<WeixinMenuEntityItem> SubItems { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }
}
