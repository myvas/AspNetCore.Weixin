using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace AspNetCore.Weixin
{
    /// <summary>
    /// 微信API访问凭证（access_token）是公众号的全局唯一票据。公众号调用各接口时都需用到该凭证。
    /// <para>支持多AppId</para>
    /// </summary>
    /// <typeparam name="TOptions">Specifies which type for of WeixinAccessTokenOptions property</typeparam>
    public class WeixinAccessTokenManager : IWeixinAccessToken
    {
        protected WeixinAccessTokenOptions _options { get; private set; }
        protected ILogger<WeixinAccessTokenManager> _logger { get; private set; }

        public WeixinAccessTokenManager(
            IOptionsSnapshot<WeixinAccessTokenOptions> optionsAccessor,
            ILoggerFactory loggerFactory)
        {
            _options = optionsAccessor.Value;
            _logger = loggerFactory.CreateLogger<WeixinAccessTokenManager>();
        }

        string IWeixinAccessToken.GetToken(bool forceRenew)
        {
            return AccessToken.Default.GetToken(_options.AppId, _options.AppSecret, forceRenew);
        }
        string IWeixinAccessToken.GetToken()
        {
            return AccessToken.Default.GetToken(_options.AppId, _options.AppSecret);
        }        
    }
}
