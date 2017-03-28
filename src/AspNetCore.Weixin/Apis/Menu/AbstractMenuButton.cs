using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetCore.Weixin
{
    /// <summary>
    /// 所有单击按钮的基类（view，click等）
    /// </summary>
    public abstract class AbstractMenuButton : MenuButton, IMenuButton
    {
        /// <summary>
        /// 按钮类型（click或view）
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="theType"></param>
        protected AbstractMenuButton(string theType)
        {
            type = theType;
        }
    }
}
