using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore;
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
        private readonly WeixinOptions _options;
        private readonly WeixinStoreOptions _storeOptions;
        private readonly IWeixinAccessToken _tokenService;
        private readonly UserApi _api;
        private readonly UserProfileApi _userProfileApi;

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
            IOptions<WeixinOptions> optionsAccessor,
            IOptions<WeixinStoreOptions> storeOptionsAccessor,
            IWeixinAccessToken tokenApi,
            UserApi api,
            UserProfileApi userProfileApi,
            ILogger<WeixinSubscriberManager<TSubscriber>> logger,
            ISubscriberStore<TSubscriber> store)
        {
            _options = optionsAccessor?.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));
            _storeOptions = storeOptionsAccessor?.Value ?? throw new ArgumentNullException(nameof(storeOptionsAccessor));
            _tokenService = tokenApi ?? throw new ArgumentNullException(nameof(tokenApi));
            _api = api ?? throw new ArgumentNullException(nameof(api));
            _userProfileApi = userProfileApi ?? throw new ArgumentNullException(nameof(userProfileApi));
            Logger = Logger;
            Store = store;
        }

        public async Task<List<TSubscriber>> GetAllAsync(bool forceRenew)
        {
            var token = await _tokenService.GetTokenAsync();
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

        public async Task<bool> SubscribeAsync(SubscribeEventReceivedEntry e)
        {
            var openId = e.FromUserName;
            var entity = await Store.FindByOpenIdAsync(openId);
            if (entity == null)
            {
                var accessToken = await _tokenService.GetTokenAsync();
                var userInfoJson = await _userProfileApi.GetUserInfo(accessToken, openId);
                if (userInfoJson.Succeeded)
                {
                    entity = (TSubscriber)userInfoJson.ToEntity();
                    await Store.CreateAsync(entity);
                    return true;
                }
                return false;
            }
            else
            {
                var accessToken = await _tokenService.GetTokenAsync();
                var userInfoJson = await _userProfileApi.GetUserInfo(accessToken, openId);
                if (userInfoJson.Succeeded)
                {
                    var entity2 = (TSubscriber)userInfoJson.ToEntity();
                    entity = entity.Update(entity2);
                    await Store.UpdateAsync(entity);
                }
                return true;
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
            var models = json.Select(x => (TSubscriber)x.ToEntity()).ToList();
            return models;
        }

        private async Task<List<TSubscriber>> StoreAllSubscribersAsync(List<UserInfoJson> json)
        {
            var models = ToModels(json);
            foreach (var model in models)
            {
                var subscriber = await Store.FindByOpenIdAsync(model.OpenId);
                if (subscriber == null)
                {
                    await Store.CreateAsync(model);
                }
                else
                {
                    subscriber = subscriber.Update(model);
                    await Store.UpdateAsync(subscriber);
                }
            }
            return models;
        }
        #endregion
    }
}
