using System;
using System.Collections.Generic;
using System.Globalization;

namespace Myvas.AspNetCore.Weixin;

public class WeixinGetApplyShopResultJson : WeixinWifiErrorJson
{
    public string applyDate { get; set; }
    public DateTime GetApplyDate()
    {
        return DateTime.ParseExact(applyDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
    }

    public List<WeixinShopApplyForm> shopList = new List<WeixinShopApplyForm>();
}
