using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin.Site.Tests.TestServers;

public class FakeServerMessageHandler : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return FakeServerBuilder.CreateTencentServer().CreateClient().SendAsync(request, cancellationToken);
    }
}
