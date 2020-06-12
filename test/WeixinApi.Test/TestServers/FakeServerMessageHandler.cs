using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin.AccessToken.Test
{
    public class FakeServerMessageHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return FakeServerBuilder.CreateTencentServer().CreateClient().SendAsync(request, cancellationToken);
        }
    }
}
