﻿namespace Myvas.AspNetCore.Weixin;

public class SetAdResultJson : WifiErrorJson
{
    /// <summary>
    /// 该次请求生产的广告id。用于查询审核结果。
    /// </summary>
    public string requestId;
}
