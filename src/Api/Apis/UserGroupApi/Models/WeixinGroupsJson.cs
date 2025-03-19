using System.Collections.Generic;

namespace Myvas.AspNetCore.Weixin;

public class WeixinGroupsJson : WeixinErrorJson
{
    public List<WeixinGroupsJson_Group> groups { get; set; }
}

public class WeixinGroupsJson_Group
{
    public int id { get; set; }
    public string name { get; set; }
    /// <summary>
    /// 此属性在CreateGroupResult的Json数据中，创建结果中始终为0
    /// </summary>
    public int count { get; set; }
}
