using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Myvas.AspNetCore.Weixin
{
    public abstract class SecureApiClient :ApiClient
    {
        protected IWeixinAccessToken _tokenProvider { get; private set; }

        protected string AppId { get { return _options.AppId; } }
        public SecureApiClient(IOptions<WeixinApiOptions> optionsAccessor, IWeixinAccessToken tokenProvider):base(optionsAccessor)
        {
            _tokenProvider = tokenProvider ?? throw new ArgumentException(nameof(tokenProvider));
        }

        public async Task<TValue> SecureGetFromJsonAsync<TValue>(string requestUriFormat, CancellationToken cancellationToken = default)
            where TValue : IWeixinError
            => await GetFromJsonAsync<TValue>(await _tokenProvider.GetTokenAsync(), requestUriFormat, cancellationToken);

        public Task<TValue> GetFromJsonAsync<TValue>(string accessToken, string requestUriFormat, CancellationToken cancellationToken = default)
            where TValue : IWeixinError
            => GetFromJsonAsync<TValue>(string.Format(requestUriFormat, accessToken), cancellationToken);

        // Post TValue (or anonymous json object), Get TResult
        public Task<TResult> PostAsJsonAsync<TValue, TResult>(string accessToken, string requestUriFormat, TValue value, 
            JsonSerializerOptions options = null, CancellationToken cancellationToken = default)
            where TResult : WeixinErrorJson
            => PostAsJsonAsync<TValue, TResult>(string.Format(requestUriFormat, accessToken), value, options, cancellationToken);

        public async Task<TResult> SecurePostAsJsonAsync<TValue, TResult>(string requestUriFormat, TValue value, 
            JsonSerializerOptions options = null, CancellationToken cancellationToken = default)
            where TResult : WeixinErrorJson
            => await PostAsJsonAsync<TValue, TResult>(await _tokenProvider.GetTokenAsync(), requestUriFormat, value, options, cancellationToken);
    }
}
