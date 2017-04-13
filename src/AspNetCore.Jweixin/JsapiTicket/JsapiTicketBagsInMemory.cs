using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetCore.Jweixin
{
    public class JsapiTicketBagsInMemory : IJsapiTicketBagsRepository
    {
        /// <summary>
        ///JsapiTicket管理类。在内存中缓存JsapiTicket。
        /// <para>将JsapiTicket存储在内存中，因此使用本类的微信应用将不支持跨进程部署，当然也不支持跨Web站点、跨服务器部署。</para>
        /// <para>初始化访问凭证数据包时，同一时间只允许一个操作，以防止在多线程环境中被重复初始化（导致重复调用API）。</para>
        /// </summary>
        private static readonly object _lockInitializing = new object();

        /// <summary>
        /// Key: 微信号AccessToken
        /// </summary>
        private static Dictionary<string,JsapiTicketBag> _JsapiTicketBags = new Dictionary<string,JsapiTicketBag>();

        public JsapiTicketBag Load(string accessToken)
        {
           JsapiTicketBag bag = new JsapiTicketBag();
            if (_JsapiTicketBags.Keys.Contains(accessToken))
            {
                bag = _JsapiTicketBags[accessToken];
            }
            return bag;
        }

        public void Store(string accessToken,JsapiTicketBag bag)
        {
            if (!_JsapiTicketBags.Keys.Contains(accessToken))
            {
                lock (_lockInitializing)
                {
                    if (!_JsapiTicketBags.Keys.Contains(accessToken))
                    {
                        _JsapiTicketBags.Add(accessToken, null);
                    }
                }
            }

            _JsapiTicketBags[accessToken] = bag;
        }

        public void SetExpired(string accessToken)
        {
           JsapiTicketBag bag = Load(accessToken);
            bag.SetExpired();
            Store(accessToken, bag);
        }

        public bool IsExpired(string accessToken)
        {
           JsapiTicketBag bag = Load(accessToken);
            return bag.IsExpired;
        }

        public string GetTicket(string accessToken)
        {
           JsapiTicketBag bag = Load(accessToken);
           return bag.JsapiTicketJson.ticket;
        }
    }
}