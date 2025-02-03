using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Myvas.AspNetCore.Weixin
{

    public class WeixinMenuItem
    {
        #region 存储/显示
        public WeixinMenuItem()
        {
            Id = Guid.NewGuid().ToString();
        }
        [JsonIgnore]
        public string Id { get; set; }
        /// <summary>
        /// 上级菜单，顶级菜单为null
        /// </summary>
        [JsonIgnore]
        public string ParentId { get; set; } = null;
        /// <summary>
        /// 序号。特别地，有多个同级菜单序号相同时，显示顺序不确定。
        /// </summary>
        [JsonIgnore]
        public int Order { get; set; }
        #endregion

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("sub_button")]
        public IList<WeixinMenuItem> SubItems { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
