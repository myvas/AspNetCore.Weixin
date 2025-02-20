using System;
using System.Collections.Generic;
using System.Text;

namespace Myvas.AspNetCore.Weixin.Abstractions.Site
{
    public class CompatibleWeixinSiteEncoder : IWeixinSiteEncoder
    {
        public string Protect(string data)
        {
            throw new NotImplementedException();
        }

        public string Unprotect(string data)
        {
            throw new NotImplementedException();
        }
    }
    public class AesWeixinSiteEncoder : IWeixinSiteEncoder
    {
        public string Protect(string data)
        {
            throw new NotImplementedException();
        }

        public string Unprotect(string data)
        {
            throw new NotImplementedException();
        }
    }
    public class ClearTextWeixinSiteEncoder : IWeixinSiteEncoder
    {
        public string Protect(string data)
        {
            return data;
        }

        public string Unprotect(string data)
        {
            return data;
        }
    }
}
