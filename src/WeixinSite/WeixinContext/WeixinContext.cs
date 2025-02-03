using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Myvas.AspNetCore.Weixin
{

    public class WeixinRequestContext
    {
        public HttpContext RequestContext { get; private set; }

        public string ToUser {get;private set;}
        public string ListenId { get; private set; }
    }
}
