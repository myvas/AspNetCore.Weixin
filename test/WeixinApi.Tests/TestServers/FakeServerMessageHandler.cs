using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin.Api.Tests.TestServers;

public class FakeServerMessageHandler : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return FakeTencentServerBuilder.CreateTencentServer().CreateClient().SendAsync(request, cancellationToken);
    }
}
