namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 创建分组返回结果
/// </summary>
public class WeixinCreateGroupResult : WeixinErrorJson
{
    public WeixinGroupsJson_Group group { get; set; }
}
