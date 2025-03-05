using System;
using System.Collections.Generic;
using System.Text;

namespace Myvas.AspNetCore.Weixin.Abstractions.Site
{
    public interface IWeixinSiteEncoder
    {
        /// <summary>
        /// Protect the data.
        /// </summary>
        /// <param name="data">The data to protect.</param>
        /// <returns>The protected data.</returns>
        string Protect(string data);

        /// <summary>
        /// Unprotect the data.
        /// </summary>
        /// <param name="data"></param>
        /// <returns>The unprotected data.</returns>
        string Unprotect(string data);
    }
}
