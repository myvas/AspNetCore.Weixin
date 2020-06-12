namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// https://developers.weixin.qq.com/doc/offiaccount/Basic_Information/Network_Detection.html
    /// </summary>
    public static class CheckNetworkOperators
    {
        /// <summary>
        /// 电信出口
        /// </summary>
        public const string ChinaNet = "CHINANET";
        /// <summary>
        /// 联通出口
        /// </summary>
        public const string Unicom = "UNICOM";
        /// <summary>
        /// 腾讯自建出口
        /// </summary>
        public const string Tencent = "CAP";
        /// <summary>
        /// 根据ip来选择运营商
        /// </summary>
        public const string Default = "DEFAULT";
    }
}
