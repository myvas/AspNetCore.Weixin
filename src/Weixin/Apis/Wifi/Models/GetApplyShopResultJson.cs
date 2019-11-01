using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Myvas.AspNetCore.Weixin
{
    public class GetApplyShopResultJson : WifiErrorJson
    {
        public string applyDate { get; set; }
        public DateTime GetApplyDate()
        {
            return DateTime.ParseExact(applyDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
        }

        public List<ShopApplyForm> shopList = new List<ShopApplyForm>();
    }
}
