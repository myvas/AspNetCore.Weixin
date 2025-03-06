using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;

namespace Myvas.AspNetCore.Weixin
{
    public class WeixinApiOptions
    {
        public string AppId { get; set; }
        public string AppSecret { get; set; }

        public WeixinAccessTokenEndpoint Endpoint { get; set; } = WeixinAccessTokenEndpoint.Default;

        /// <summary>
        /// Used to communicate with the remote tencent server.
        /// </summary>
        public HttpClient Backchannel { get; set; }
    }
}
