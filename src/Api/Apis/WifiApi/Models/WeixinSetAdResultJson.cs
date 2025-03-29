namespace Myvas.AspNetCore.Weixin;

public class WeixinSetAdResultJson : WeixinWifiErrorJson
{
    /// <summary>
    /// 该次请求生产的广告id。用于查询审核结果。
    /// </summary>
    public string requestId;
}
