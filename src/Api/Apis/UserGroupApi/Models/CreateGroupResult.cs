namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// 创建分组返回结果
    /// </summary>
    public class CreateGroupResult : WeixinErrorJson
    {
        public GroupsJson_Group group { get; set; }
    }
}
