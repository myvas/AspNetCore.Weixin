using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.Weixin
{
    public abstract class BaseContext
    {
        protected BaseContext(HttpContext context)
        {
            HttpContext = context;
        }

        public HttpContext HttpContext { get; private set; }

        public HttpRequest Request
        {
            get { return HttpContext.Request; }
        }

        public HttpResponse Response
        {
            get { return HttpContext.Response; }
        }
    }
}
