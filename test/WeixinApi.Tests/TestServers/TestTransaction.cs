﻿using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Xml.Linq;

namespace Myvas.AspNetCore.Weixin.Api.Tests;

public class TestTransaction
{
    public HttpRequestMessage Request { get; set; }
    public HttpResponseMessage Response { get; set; }

    public IList<string> SetCookie { get; set; }

    public string ResponseText { get; set; }
    public XElement ResponseElement { get; set; }

    public string AuthenticationCookieValue
    {
        get
        {
            if (SetCookie != null && SetCookie.Count > 0)
            {
                var authCookie = SetCookie.SingleOrDefault(c => c.Contains(".AspNetCore." + TestServerExtensions.CookieAuthenticationScheme + "="));
                if (authCookie != null)
                {
                    return authCookie.Substring(0, authCookie.IndexOf(';'));
                }
            }

            return null;
        }
    }

    public string FindClaimValue(string claimType, string issuer = null)
    {
        var claim = ResponseElement.Elements("claim")
            .SingleOrDefault(elt => elt.Attribute("type").Value == claimType &&
                (issuer == null || elt.Attribute("issuer").Value == issuer));
        if (claim == null)
        {
            return null;
        }
        return claim.Attribute("value").Value;
    }

    public string FindTokenValue(string name)
    {
        var claim = ResponseElement.Elements("token")
            .SingleOrDefault(elt => elt.Attribute("name").Value == name);
        if (claim == null)
        {
            return null;
        }
        return claim.Attribute("value").Value;
    }
}
