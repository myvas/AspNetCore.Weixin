using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetCore.Weixin
{
    /// <summary>
    /// jsapi_ticket是公众号用于调用微信JS接口的临时票据。
    /// <para>正常情况下，jsapi_ticket的有效期为7200秒，通过access_token来获取。</para>
    /// <para>由于获取jsapi_ticket的api调用次数非常有限，频繁刷新jsapi_ticket会导致api调用受限，影响自身业务，开发者必须在自己的服务全局缓存jsapi_ticket。</para>
    /// <para>支持多AccessToken</para>
    /// </summary>
    public class JsapiTicket
    {
        #region 持久化支持
        /// <summary>
        /// 是否启用持久化功能
        /// </summary>
        public bool IsNotPersistent { get { return _JsapiTicketBags is JsapiTicketBagsInMemory; } }
        /// <summary>
        /// 存储访问凭证数据包
        /// <para>Key: AccessToken</para>
        /// </summary>
        private static IJsapiTicketBagsRepository _JsapiTicketBags = new JsapiTicketBagsInMemory();
        static readonly object _lockAccessing = new object();
        
        public void UnregisterPersistent()
        {
            lock (_lockAccessing)
            {
                _JsapiTicketBags = new JsapiTicketBagsInMemory();
            }
        }
        #endregion


        #region 多线程安全的Singleton
        private volatile static JsapiTicket _instance = null;
        private static readonly object _lockHelper = new object();
        private JsapiTicket() { }
        //  COMMENTED BY CODEIT.RIGHT
        //        ~JsapiTicket() { }
        /// <summary>
        /// Singleton
        /// </summary>
        public static JsapiTicket Default
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockHelper)
                    {
                        if (_instance == null)
                            _instance = new JsapiTicket();
                    }
                }
                return _instance;
            }

        }
        #endregion

        static readonly object _lockApiCalling = new object();
        /// <summary>
        /// 获取微信API访问凭证。仅在需要时调用微信API接口，即：若凭证尚在有效期内，则直接取回上一次得到的凭证。
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="forceRenew">强制立即取新。这将废弃并替换旧的，而不管是否旧的令牌是否过期。</param>
        /// <returns>微信API访问凭证</returns>
        public string GetTicket(string accessToken, bool forceRenew = false)
        {
            //bool forceRenew = false;
            if (forceRenew)
            {
                //立即过期
                _JsapiTicketBags.SetExpired(accessToken);
            }
            else
            {
                forceRenew = _JsapiTicketBags.IsExpired(accessToken);
            }

            //forceRenew = CheckTokenExpire();

            if (forceRenew)
            {
                lock (_lockApiCalling)
                {
                    //再次检查，通常在等待上一锁释放期间，过期状态已经发生改变。若已改变，则不应再调用API去刷新Token！
                    if (_JsapiTicketBags.IsExpired(accessToken))
                    {
                        JsapiTicketJson json = JsapiTicketApi.GetJsapiTicket(accessToken).Result;
                        if (json != null
                            && !string.IsNullOrEmpty(json.ticket))
                        {
                            _JsapiTicketBags.Store(accessToken, new JsapiTicketBag()
                            {
                                JsapiTicketJson = json,
                                AccessToken = accessToken,
                                CreateTime = DateTime.Now
                            });
                        }
                    }
                }
            }

            return _JsapiTicketBags.GetTicket(accessToken);
        }
    }
}
