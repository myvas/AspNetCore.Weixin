using System;
using System.Globalization;

namespace Myvas.AspNetCore.Weixin;

public class ShopApplyForm
{
    /// <summary>
    /// 需要安装wifi的门店id，在设置AP接口同步回来
    /// <para>例如：SHOPID1</para>
    /// </summary>
    public string shopId { get; set; }

    /// <summary>
    /// 门店名称
    /// </summary>
    public string shopName { get; set; }

    public string shopAddress { get; set; }

    /// <summary>
    /// 门店联系人
    /// </summary>
    public string contact { get; set; }

    /// <summary>
    /// 门店联系人电话
    /// </summary>
    public string contactPhone { get; set; }

    /// <summary>
    /// 申请时间
    /// </summary>
    public string applyTime { get; set; }

    public DateTime GetApplyTime()
    {
        return DateTime.ParseExact(applyTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None);
    }
}
