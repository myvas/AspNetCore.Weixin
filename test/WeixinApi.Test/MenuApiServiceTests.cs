using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Myvas.AspNetCore.Weixin.AccessToken.Test
{
    public class MenuApiServiceTests
    {
        private readonly TestServer _server;
        public MenuApiServiceTests()
        {
            _server = FakeServerBuilder.CreateTencentServer();
        }

        [Fact]
        public async Task PublishMenuShouldSuccess()
        {
            var services = new ServiceCollection();
            services.AddWeixinApi(o =>
            {
                o.AppId = "APPID";
                o.AppSecret = "APPSECRET";
                o.Backchannel = _server.CreateClient();
            });
            var serviceProvider = services.BuildServiceProvider();
            var api = serviceProvider.GetRequiredService<IWeixinMenuApi>();
            var menu = new WeixinMenu("unittest");
            var menu1 = menu.AddItem(new WeixinMenuItemClick()
            {
                Name = "每日之歌",
                Key = "V1001_MUSIC_DAILY"
            });
            var menu2 = menu.AddItem(new WeixinMenuItem
            {
                Name = "菜单"
            });
            var menu21 = menu.AddItem(new WeixinMenuItemView
            {
                ParentId = menu2.Id,
                Name = "搜索",
                Url = "http://www.soso.com"
            });
            var menu22 = menu.AddItem(new WeixinMenuItemView
            {
                ParentId = menu2.Id,
                Name = "视频",
                Url = "http://v.qq.com"
            });
            var menu23 = menu.AddItem(new WeixinMenuItemClick
            {
                ParentId = menu2.Id,
                Name = "为我们点赞",
                Key = "V1001_LIKE"
            });
            var json = await api.PublishMenuAsync(menu);

            Assert.True(json.Succeeded);
        }

        [Fact]
        public async Task GetMenuShouldSuccess()
        {
            var services = new ServiceCollection();
            services.AddWeixinApi(o =>
            {
                o.AppId = "APPID";
                o.AppSecret = "APPSECRET";
                o.Backchannel = _server.CreateClient();
            });
            var serviceProvider = services.BuildServiceProvider();
            var api = serviceProvider.GetRequiredService<IWeixinMenuApi>();
            var menu = await api.GetMenuAsync();

            Assert.NotNull(menu);
            Assert.NotNull(menu.Items);
            Assert.Equal(5, menu.Items.Count);

            var serialized = new WeixinMenuJsonSerializerForApi().Serialize(menu);
            Debug.WriteLine(serialized);
        }

        [Fact]
        public async Task DeleteMenuShouldSuccess()
        {
            var services = new ServiceCollection();
            services.AddWeixinApi(o =>
            {
                o.AppId = "APPID";
                o.AppSecret = "APPSECRET";
                o.Backchannel = _server.CreateClient();
            });
            var serviceProvider = services.BuildServiceProvider();
            var api = serviceProvider.GetRequiredService<IWeixinMenuApi>();
            var json = await api.DeleteMenuAsync();

            Assert.True(json.Succeeded);
        }
    }
}
