namespace Myvas.AspNetCore.Weixin
{
    public static class WeixinCustomErrors
    {
        public static IWeixinError GenericError = new WeixinErrorJson(50000, "获取access_token接口调用失败");

        public static IWeixinError UnknownResponse = new WeixinErrorJson(50001, "获取access_token接口没有正确返回");

        public static IWeixinError Busy = new WeixinErrorJson(-1, "系统繁忙，请稍后再试");

        public static IWeixinError AppSecretError = new WeixinErrorJson(40001, "AppSecret错误或者AppSecret不属于这个公众号");

        public static IWeixinError IllegalGrantType = new WeixinErrorJson(40001, "grant_type字段值非法（请确保为client_credential）");

        public static IWeixinError InvalidAppId = new WeixinErrorJson(40013, "AppID无效");

        public static IWeixinError ValidateAppidFailed = new WeixinErrorJson(40164, "调用接口的IP地址不在白名单中");
    }
}
