using System;
using System.Collections.Generic;
using System.Text;

namespace Myvas.AspNetCore.Weixin
{
    public class WeixinAccessTokenEndpoint
    {
        public static readonly WeixinAccessTokenEndpoint Default = new WeixinAccessTokenEndpoint("https://api.weixin.qq.com/cgi-bin/token");

        public Uri Uri { get; private set; }

        public WeixinAccessTokenEndpoint(string uri)
        {
            Uri = new Uri(uri);
        }

        public WeixinAccessTokenEndpoint(Uri uri)
        {
            Uri = uri;
        }

        public WeixinAccessTokenEndpoint ChangeServer(string host, int port = 80)
        {
            var builder = new UriBuilder(Uri);
            builder.Host = host;
            builder.Port = port;
            Uri = builder.Uri;
            return this;
        }

        public override string ToString()
        {
            return Uri.AbsoluteUri;
        }
    }
}
