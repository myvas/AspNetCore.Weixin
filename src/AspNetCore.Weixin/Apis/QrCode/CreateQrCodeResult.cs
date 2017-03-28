using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetCore.Weixin
{
    /// <summary>
    /// 二维码创建返回结果
    /// </summary>
    public class CreateQrCodeResult : WeixinErrorJson
    {
        /// <summary>
        /// 获取的二维码ticket，凭借此ticket可以在有效时间内换取二维码。
        /// </summary>
        public string ticket { get; set; }
        /// <summary>
        /// 该二维码有效时间，以秒为单位。 最大不超过604800（即7天）。
        /// </summary>
        public int expire_seconds { get; set; }
        /// <summary>
        /// 二维码图片解析后的地址，开发者可根据该地址自行生成需要的二维码图片。
        /// </summary>
        public string url { get; set; }
    }
}
