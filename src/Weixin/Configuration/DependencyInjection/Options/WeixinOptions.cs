using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Myvas.AspNetCore.Weixin.Configuration;

namespace Myvas.AspNetCore.Weixin
{
    public class WeixinOptions
    {
        public string AppId { get; set; }
        public string AppSecret { get; set; }

        /// <summary>
        /// Gets or sets the caching options.
        /// </summary>
        /// <value>
        /// The caching options.
        /// </value>
        public CachingOptions Caching { get; set; } = new CachingOptions();
    }
}
