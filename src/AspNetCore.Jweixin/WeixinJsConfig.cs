using AspNetCore.Weixin;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.Jweixin
{

    public class WeixinJsConfig
    {
        public bool debug = false;
        public string appId;
        public long timestamp = WeixinTimestampHelper.FromLocalTime(DateTime.Now);
        public string nonceStr = Guid.NewGuid().ToString("N");
        public string signature;
        public string[] jsApiList = new[] {
            "onMenuShareTimeline",
            "onMenuShareAppMessage",
            "onMenuShareQQ",
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
            elements.Add("url", refererUrl);
            elements.Add("nonceStr", this.nonceStr);
            elements.Add("timestamp", this.timestamp);

            //var signFields = new string[]{"nonceStr","timestamp"};
            //var fieldInfos = this.GetType().GetFields()
            //    .Where(x => (signFields.Contains(x.Name)));
            //foreach (var fi in fieldInfos)
            //{
            //    elements.Add(fi.Name, fi.GetValue(this));
            //}

            signature = JsSignatureApi.CalculateJsSignature(elements);

            //JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            //string json = oSerializer.Serialize(this);
            //return json;
            return WeixinJsonHelper.Serialize(this);
        }
    }
}
