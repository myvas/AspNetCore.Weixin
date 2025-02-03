using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using System;
using System.Collections;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Myvas.AspNetCore.Weixin.EntityFrameworkCore.InMemory.Test
{
    public class WeixinSpecificationTestBase<TWeixinSubscriber, TKey> : SubscriberManagerSpecificationTestBase<TWeixinSubscriber, TKey>
        where TWeixinSubscriber : class
        where TKey : IEquatable<TKey>
    {
    }

    public abstract class SubscriberManagerSpecificationTestBase<TWeixinSubscriber, TKey>
        where TWeixinSubscriber : class
        where TKey : IEquatable<TKey>
    {
        protected readonly WeixinErrorDescriber _errorDescriber = new WeixinErrorDescriber();

        protected virtual void SetupWeixinServices(IServiceCollection services, object context)
            => SetupBuilder(services, context);

        protected virtual WeixinSiteBuilder SetupBuilder(IServiceCollection services, object context)
        {
            services.AddHttpContextAccessor();
            services.AddDataProtection();
            var builder = services.AddWeixin(options =>
            {
                options.AppId = "weixinappid";
                options.AppSecret = "weixinappsecret";
            }).AddDefaultTokenProviders();
            AddSubscriberStore(services, context);
            services.AddLogging();
            services.AddSingleton < ILogger<SubscriberManager<TKey>>(new TestLogger<SubscriberManager<TKey>>());
            return builder;
        }

        protected virtual SubscriberManager<TKey> CreateManager(object context =null, IServiceCollection services = null, Action<IServiceCollection> configureServices = null)
        {
            if (services == null)
            {
                services = new ServiceCollection();
            }
            if (context == null)
            {
                context = CreateTestContext();
            }
            SetupWeixinServices(services, context);
            configureServices?.Invoke(services);
            return services.BuildServiceProvider().GetService<SubscriberManager<TKey>>();
        }

        protected abstract object CreateTestContext();

        protected abstract void AddSubscriberStore(IServiceCollection services, object context = null);

        protected abstract TWeixinSubscriber CreateTestSubscriber(string openIdPrefix = "", bool userOpenIdPrefixAsOpenId = false);

        protected abstract Expression<Func<TWeixinSubscriber, bool>> OpenIdEqualsPredicate(string openId);

        [Fact]
        public async Task CanDeleteSubscriber()
        {
            var manager = CreateManager();
            var subscriber = CreateTestSubscriber();
            WeixinResultAssert.IsSuccess(await manager.CreateAsync(subscriber));
            var openId = await manager.GetOpenIdAsync(subscriber);
            WeixinResultAssert.IsSuccess(await manager.DeleteAsync(subscriber));
            Assert.Null(await manager.FindByOpenIdAsync(openId));
        }
    }
}