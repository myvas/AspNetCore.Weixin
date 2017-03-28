using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.Weixin
{
    /// <summary>
    /// Base class for other Weixin contexts.
    /// </summary>
    public class BaseWeixinContext : BaseContext
    {
        /// <summary>
        /// Initializes a <see cref="BaseWeixinContext"/>
        /// </summary>
        /// <param name="context">The HTTP environment</param>
        /// <param name="options">The options for <see cref="WeixinWelcomePageMiddleware"/></param>
        public BaseWeixinContext(HttpContext context, WeixinWelcomePageOptions options)
            : base(context)
        {
            Options = options;
        }

        public WeixinWelcomePageOptions Options { get; }
    }
}
