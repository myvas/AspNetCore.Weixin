using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// GetMenu返回的Json结果
    /// </summary>
    public class GetMenuResult
    {
        public ButtonGroup menu { get; set; }

        public GetMenuResult()
        {
            menu = new ButtonGroup();
        }
    }
}
