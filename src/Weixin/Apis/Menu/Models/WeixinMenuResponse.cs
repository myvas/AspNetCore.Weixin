using System;
using System.Text.Json;

namespace Myvas.AspNetCore.Weixin.Menu
{
    public class WeixinMenuResponse
    {
        public JsonDocument Response { get; set; }
        public WeixinException Error { get; set; }

        public bool HasError { get; private set; }

        private WeixinMenuResponse(JsonDocument response)
        {
            Response = response;

            var errcode = response.RootElement.GetInt32("errcode");
            HasError = (errcode != 0);
            if (!HasError)
            {
            }
            else
            {
                var errmsg = response.RootElement.GetString("errmsg");
                Error = new WeixinException(errmsg);
            }

        }

        private WeixinMenuResponse(Exception ex)
        {
        }

        public static WeixinMenuResponse Parse(JsonDocument response)
        {
            return new WeixinMenuResponse(response);
        }
        public static WeixinMenuResponse Exception(Exception ex)
        {
            return new WeixinMenuResponse(ex);
        }
    }
}
