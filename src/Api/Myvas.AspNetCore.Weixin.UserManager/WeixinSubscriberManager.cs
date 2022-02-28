using Microsoft.Extensions.Options;
using Myvas.AspNetCore.Weixin.AccessTokenServer.Stores;
using Myvas.AspNetCore.Weixin.Options;
using System;
using System.Threading.Tasks;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.Mappers;
using Myvas.AspNetCore.Weixin.Models;

namespace Myvas.AspNetCore.Weixin
{
    public class WeixinSubscriberManager : IWeixinSubscriberManager
    {
        private readonly WeixinSubscriberManagerOptions _options;
        private readonly IWeixinAccessToken _tokenApi;
        private readonly UserApi _api;
        private readonly IWeixinUserStore _store;

        public WeixinSubscriberManager(
            IOptions<WeixinSubscriberManagerOptions> optionsAccessor,
            IWeixinAccessToken tokenApi,
            UserApi api,
            IWeixinUserStore store)
        {
            _options = optionsAccessor?.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));
            _tokenApi = tokenApi ?? throw new ArgumentNullException(nameof(tokenApi));
            _api = api ?? throw new ArgumentNullException(nameof(api));
            _store = store;
        }

        public async Task<List<Models.WeixinSubscriber>> GetAllAsync(bool forceRenew)
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
                var models = await _store.GetAllAsync(null);
                return models.ToList();
            }
        }

        #region private methods
        private async Task<List<UserInfoJson>> FetchAllSubscribersAsync(string token)
        {
            var json = await _api.GetAllUserInfo(token);
            return json;
        }

        private static List<Models.WeixinSubscriber> ToModels(List<UserInfoJson> json)
        {
            var models = json.Select(x => new Models.WeixinSubscriber
            {
                OpenId = x.OpenId,
                UnionId = x.UnionId,
                SubscribedTime = WeixinTimestampHelper.ToUtcTime(x.subscribe_time),
                Language = x.language,
                Unsubscribed = x.subscribe != 1,
                Nickname = x.nickname,
                Gender = x.sex == null ? WeixinGender.Unknown : (x.sex!.Value == 0 ? WeixinGender.Male : WeixinGender.Female)
            }).ToList();
            return models;
        }

        private async Task<List<WeixinSubscriber>> StoreAllSubscribersAsync(List<UserInfoJson> json)
        {
            var models = ToModels(json);
            foreach (var model in models)
                await _store.StoreAsync(model);
            return models;
        }
        #endregion
    }
}
