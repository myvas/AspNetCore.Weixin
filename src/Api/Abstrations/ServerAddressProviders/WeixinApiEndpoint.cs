using System;

namespace Myvas.AspNetCore.Weixin;

public class WeixinApiEndpoint
{
    /// <summary>
    /// Main data of this class
    /// </summary>
    public Uri Uri { get; private set; }

    /// <summary>
    /// Current selected server. The candidate servers are in <see cref="WeixinApiServers"/>. 
    /// </summary>
    public string WeixinApiServer { get => Uri.Host; }

    public WeixinApiEndpoint(string host, string pathValue)
    {
        host ??= WeixinApiServers.Default;
        if (host.StartsWith("http"))
        {
            Uri = new Uri(new Uri(host), pathValue);
        }
        else
        {
            var uriBuiler = new UriBuilder("https", host, 443, pathValue);
            Uri = uriBuiler.Uri;
        }
    }

    public WeixinApiEndpoint()
        : this(WeixinApiServers.Default, "")
    {
    }

    public WeixinApiEndpoint(string uri)
    {
        Uri = new Uri(uri);
    }

    public WeixinApiEndpoint(Uri uri)
    {
        Uri = uri;
    }

    public WeixinApiEndpoint ChangeServer(string host)
    {
        var builder = new UriBuilder(Uri)
        {
            Host = host
        };
        Uri = builder.Uri;
        return this;
    }

    public WeixinApiEndpoint SetPathAndQuery(string pathAndQuery)
    {
        var builder = new UriBuilder(Uri)
        {
            Path = pathAndQuery
        };
        Uri = builder.Uri;
        return this;
    }

    public override string ToString()
    {
        return Uri.AbsoluteUri;
    }
}
