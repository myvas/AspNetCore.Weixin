using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// 子菜单
    /// </summary>
    public class SubMenuButton : MenuButton, IMenuButton
    {
        /// <summary>
        /// 子按钮数组，按钮个数应为2~5个
        /// </summary>
        public List<AbstractMenuButton> sub_button { get; set; }

        public SubMenuButton()
        {
            sub_button = new List<AbstractMenuButton>();
        }

        public SubMenuButton(string name)
            : this()
        {
            base.name = name;
        }
    }
}
