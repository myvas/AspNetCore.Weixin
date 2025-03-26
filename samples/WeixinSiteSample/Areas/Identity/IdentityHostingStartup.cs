[assembly: HostingStartup(typeof(WeixinSiteSample.Areas.Identity.IdentityHostingStartup))]
namespace WeixinSiteSample.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}