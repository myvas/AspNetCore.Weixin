namespace Myvas.AspNetCore.Weixin
{
    public class WeixinMessagingOptions
    {
        /// <summary>
        /// default is 3
        /// </summary>
        public int MaxRetryTimes { get; set; } = 3;
    }
}
