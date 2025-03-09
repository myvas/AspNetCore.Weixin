namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 获取用户分组ID返回结果
/// </summary>
public class GetGroupIdResult : WeixinErrorJson
{
    public int groupid { get; set; }
}
