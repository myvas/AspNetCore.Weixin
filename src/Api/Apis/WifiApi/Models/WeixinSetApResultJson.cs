using System.Collections.Generic;

namespace Myvas.AspNetCore.Weixin;

public class WeixinSetApResultJson : WeixinWifiErrorJson
{
    /// <summary>
    /// 该次请求生产的广告id。用于查询审核结果。
    /// </summary>
    public List<string> errAplist;
}
