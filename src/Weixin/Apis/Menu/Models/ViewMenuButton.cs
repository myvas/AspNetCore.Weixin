using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// Url按键
    /// </summary>
    public class ViewMenuButton : AbstractMenuButton
    {
        /// <summary>
        /// 类型为view时必须
        /// 网页链接，用户点击按钮可打开链接，不超过256字节
        /// </summary>
        public string url { get; set; }

        public ViewMenuButton()
            : base(ButtonType.view.ToString())
        {
        }
    }
}
