using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.Weixin
{
	public class WeixinJsConfig
	{
		public bool debug = false;
		public string appId;
		public long timestamp = WeixinTimestampHelper.FromLocalTime(DateTime.Now);
		public string nonceStr = Guid.NewGuid().ToString("N");
		public string signature;
		public string[] jsApiList = new[] {
			"updateAppMessageShareData",
			"updateTimelineShareData",
			"onMenuShareTimeline", //即将废弃 1.4.0
			"onMenuShareAppMessage", //即将废弃 1.4.0
			"onMenuShareQQ", //即将废弃 1.4.0
			"onMenuShareWeibo",
			"onMenuShareQZone",
			"startRecord",
			"stopRecord",
			"onVoiceRecordEnd",
			"playVoice",
			"pauseVoice",
			"stopVoice",
			"onVoicePlayEnd",
			"uploadVoice",
			"downloadVoice",
			"chooseImage",
			"previewImage",
			"uploadImage",
			"downloadImage",
			"translateVoice",
			"getNetworkType",
			"openLocation",
			"getLocation",
			"hideOptionMenu",
			"showOptionMenu",
			"hideMenuItems",
			"showMenuItems",
			"hideAllNonBaseMenuItem",
			"showAllNonBaseMenuItem",
			"closeWindow",
			"scanQRCode",
			"chooseWXPay",
			"openProductSpecificView",
			"addCard",
			"chooseCard",
			"openCard"
		};

		public string ToJson(string jsapiTicket, string refererUrl)
		{
			var elements = new Dictionary<string, object>();
			elements.Add("jsapi_ticket", jsapiTicket);
			elements.Add("nonceStr", this.nonceStr);//nonceStr
			elements.Add("timestamp", this.timestamp);
			elements.Add("url", refererUrl);

			signature = WeixinJssdkSignatureHelper.CalculateJsSignature(elements);

			return WeixinJsonHelper.Serialize(this);
		}
	}
}
