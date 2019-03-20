using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetCore.Weixin
{
    public class AccessTokenBagsInMemory : IAccessTokenBagsRepository
    {
        /// <summary>
        /// AccessToken管理类。在内存中缓存AccessToken。
        /// <para>将AccessToken存储在内存中，因此使用本类的微信应用将不支持跨进程部署，当然也不支持跨Web站点、跨服务器部署。</para>
        /// <para>初始化访问凭证数据包时，同一时间只允许一个操作，以防止在多线程环境中被重复初始化（导致重复调用API）。</para>
        /// </summary>
        private static readonly object _lockBagInitializing = new object();

        /// <summary>
        /// Key: 微信号AppId
        /// </summary>
        private static Dictionary<string, AccessTokenBag> _AccessTokenBags = new Dictionary<string, AccessTokenBag>();

        public AccessTokenBag Load(string appId)
        {
            if (string.IsNullOrEmpty(appId))
            {
                throw new ArgumentNullException(nameof(appId));
            }

            AccessTokenBag bag = new AccessTokenBag();
            if (_AccessTokenBags.Keys.Contains(appId))
            {
                bag = _AccessTokenBags[appId];
            }
            return bag;
        }

        public void Store(string appId, AccessTokenBag bag)
        {
            if (!_AccessTokenBags.Keys.Contains(appId))
            {
                lock (_lockBagInitializing)
                {
                    if (!_AccessTokenBags.Keys.Contains(appId))
                    {
                        _AccessTokenBags.Add(appId, null);
                    }
                }
            }

            _AccessTokenBags[appId] = bag;
        }

        public void SetExpired(string appId)
        {
            AccessTokenBag bag = Load(appId);
            bag.SetExpired();
            Store(appId, bag);
        }

        public bool IsExpired(string appId)
        {
            AccessTokenBag bag = Load(appId);
            return bag.IsExpired;
        }

        public string GetToken(string appId)
        {
            AccessTokenBag bag = Load(appId);
            return bag.AccessTokenJson.access_token;
        }
    }
}