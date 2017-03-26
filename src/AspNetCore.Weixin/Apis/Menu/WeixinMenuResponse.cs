using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin.Menu
{
    public class WeixinMenuResponse
    {
        public JObject Response { get; set; }
        public WeixinException Error { get; set; }

        public bool HasError { get; private set; }

        private WeixinMenuResponse(JObject response)
        {
            Response = response;

            var errcode = response.Value<int>("errcode");
            HasError = (errcode != 0);
            if (!HasError)
            {
            }
            else
            {
                var errmsg = response.Value<string>("errmsg");
                Error = new WeixinException(errmsg);
            }

        }

        private WeixinMenuResponse(Exception ex)
        {
        }

        public static WeixinMenuResponse Parse(JObject response)
        {
            return new WeixinMenuResponse(response);
        }
        public static WeixinMenuResponse Exception(Exception ex)
        {
            return new WeixinMenuResponse(ex);
        }
    }
}
