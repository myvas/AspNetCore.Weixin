namespace Myvas.AspNetCore.Weixin
{
    public class WeixinRequestLocation : WeixinRequest, IWeixinRequest
    {
        public override RequestMsgType MsgType
        {
            get { return RequestMsgType.location; }
        }

        /// <summary>
        /// 地理位置纬度Location_X
        /// </summary>
        public decimal Latitude { get; set; }
        /// <summary>
        /// 地理位置经度Location_Y
        /// </summary>
        public decimal Longitude { get; set; }
        public decimal Scale { get; set; }
        public string Label { get; set; }
    }


}
