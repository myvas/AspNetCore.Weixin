using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.Options;
using Myvas.AspNetCore.Weixin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin
{
    public class WeixinSubscriberManager<TSubscriber>
        where TSubscriber : Subscriber, new()
    {
        private readonly WeixinStoreOptions _options;
        private readonly IWeixinAccessToken _tokenApi;
        private readonly UserApi _api;

        /// <summary>
        /// Gets or sets the persistence store the manager operates over.
        /// </summary>
        protected internal ISubscriberStore<TSubscriber> Store { get; set; }

        /// <summary>
        /// The <see cref="ILogger"/> used to log messages from the manager.
        /// </summary>
        /// <value>
        /// The <see cref="ILogger"/> used to log messages from the manager.
        /// </value>
        public virtual ILogger Logger { get; set; }

        public WeixinSubscriberManager(
            IOptions<WeixinStoreOptions> optionsAccessor,
            IWeixinAccessToken tokenApi,
            UserApi api,
            ILogger<WeixinSubscriberManager<TSubscriber>> logger,
            ISubscriberStore<TSubscriber> store)
        {
            _options = optionsAccessor?.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));
            _tokenApi = tokenApi ?? throw new ArgumentNullException(nameof(tokenApi));
            _api = api ?? throw new ArgumentNullException(nameof(api));
            Logger = Logger;
            Store = store;
        }

        public async Task<List<TSubscriber>> GetAllAsync(bool forceRenew)
        {
            var token = await _tokenApi.GetTokenAsync();
            if (forceRenew)
            {
                var json = await FetchAllSubscribersAsync(token);
                var models = await StoreAllSubscribersAsync(json);
                return models;
            }
            else
            {
                var count = await Store.GetSubscribersCountAsync();
                var models = await Store.GetSubscribersAsync(count, 0);
                return models.ToList();
            }
        }

        #region private methods
        private async Task<List<UserInfoJson>> FetchAllSubscribersAsync(string token)
        {
            var json = await _api.GetAllUserInfo(token);
            return json;
        }

        private static List<TSubscriber> ToModels(List<UserInfoJson> json)
        {
            var models = json.Select(x => new TSubscriber
            {
                OpenId = x.OpenId,
                UnionId = x.UnionId,
                SubscribedTime = x.SubscribeTime,
                Language = x.Language,
                Unsubscribed = x.Unsubscribed,
                Nickname = x.NickName,
                Gender = x.Gender
            }).ToList();
            return models;
        }

        private async Task<List<TSubscriber>> StoreAllSubscribersAsync(List<UserInfoJson> json)
        {
            var models = ToModels(json);
            foreach (var model in models)
                await Store.AddSubscriberAsync(model, null);
            return models;
        }
        #endregion
    }
}
