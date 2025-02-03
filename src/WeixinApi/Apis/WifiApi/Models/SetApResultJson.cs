using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Myvas.AspNetCore.Weixin
{
    public class SetApResultJson : WifiErrorJson
    {
        /// <summary>
        /// 该次请求生产的广告id。用于查询审核结果。
        /// </summary>
        public List<string> errAplist;
    }
}
