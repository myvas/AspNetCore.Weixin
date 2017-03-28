using System.Collections.Generic;

namespace AspNetCore.Weixin
{
    /// <summary>
    /// 微信WiFi无线访问点（AP）信息
    /// </summary>
    public class Ad
    {
        /// <summary>
        /// 广告编号
        /// <para>可将AP关联至已设置的广告，即一个广告可以关联到多门店，后续关联的门店不用再审核。</para>
        /// <para>1.若设置了该字段，则deviceNos的AP关联至该广告，不新增广告；</para>
        /// <para>2.否则新增广告</para>
        /// </summary>
        public string advertiseId;

        /// <summary>
        /// LOGO url
        /// </summary>
        public string logo;

        /// <summary>
        /// 背景图url
        /// </summary>
        public string backgroundImg;

        /// <summary>
        /// 品牌名
        /// <para>若设置了该字段，则广告上展示brandName</para>
        /// <para>否则展示门店名称(storeName)</para>
        /// </summary>
        public string brandName;

        /// <summary>
        /// 微信号（公众号帐号）
        /// <para>必须。调用时将校验此域。</para>
        /// </summary>
        public string mpAccount;

        /// <summary>
        /// 设置广告内容的APlist。其值是一个json串
        /// </summary>
        public List<string> deviceNos = new List<string>();
        
        public void SetAdDetail(AdDetail detail)
        {
            if (detail is AdDetail_Template1)
            {
                adTemplet = 1;
                adDetail = (detail as AdDetail_Template1).ToJson();
            }
            else if (detail is AdDetail_Template2)
            {
                adTemplet = 2;
                adDetail = (detail as AdDetail_Template2).ToJson();
            }
        }

        /// <summary>
        /// 广告模板。
        /// <para>可选的值有：1-模板1， 2-模板2</para>
        /// <para>通过调用方法SetAdDetail(...)进行设置</para>
        /// </summary>
        public int adTemplet;

        /// <summary>
        /// 广告内容信息。其值是一个json串
        /// <para>其数据格式因adTemplet不同而略有差异。</para>
        /// <para>通过调用方法SetAdDetail(...)进行设置</para>
        /// </summary>
        public string adDetail;
    }
}
