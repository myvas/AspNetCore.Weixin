using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetCore.Weixin
{
    /// <summary>
    /// 上报地理位置事件
    /// </summary>
    public class LocationEventReceivedEventArgs : EventReceivedEventArgs
    {
        /// <summary>
        /// 纬度
        /// <example>23.134521</example>
        /// </summary>
        public decimal Latitude { get; set; }

        /// <summary>
        /// 经度
        /// <example>113.358803</example>
        /// </summary>
        public decimal Longitude { get; set; }

        /// <summary>
        /// 地理位置精度
        /// <example>40.000000</example>
        /// </summary>
        public decimal Precision { get; set; }

        /// <summary>
        /// 广州市番禺区天安科技园
        /// </summary>
        public string Label { get; set; }
    }
}
