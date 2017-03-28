using System;
using System.Threading.Tasks;

namespace AspNetCore.Weixin
{
    /// <summary>
    /// 微信API访问凭证（access_token）是公众号的全局唯一票据。公众号调用各接口时都需用到该凭证。
    /// <para>支持多AppId</para>
    /// </summary>
    public class AccessToken
    {
        #region 持久化支持
        /// <summary>
        /// 是否启用持久化功能
        /// </summary>
        public bool IsNotPersistent { get { return _AccessTokenBags is AccessTokenBagsInMemory; } }
        /// <summary>
        /// 存储访问凭证数据包
        /// <para>Key: AppId</para>
        /// </summary>
        private static IAccessTokenBagsRepository _AccessTokenBags = new AccessTokenBagsInMemory();
        static readonly object _lockTokenAccessing = new object();

        /// <summary>
        /// 共享文件方式存取
        /// </summary>
        /// <param name="folderPath"></param>
        public void RegisterFile(string folderPath)
        {
            lock (_lockTokenAccessing)
            {
                _AccessTokenBags = new AccessTokenBagsInFile(folderPath);
            }
        }

        public void UnregisterPersistent()
        {
            lock (_lockTokenAccessing)
            {
                _AccessTokenBags = new AccessTokenBagsInMemory();
            }
        }
        #endregion


        #region 多线程安全的Singleton
        private volatile static AccessToken _instance = null;
        private static readonly object _lockHelper = new object();
        private AccessToken() { }
        //  COMMENTED BY CODEIT.RIGHT
        //        ~AccessToken() { }
        /// <summary>
        /// Singleton
        /// </summary>
        public static AccessToken Default
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockHelper)
                    {
                        if (_instance == null)
                            _instance = new AccessToken();
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
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <param name="forceRenew">强制立即取新。这将废弃并替换旧的，而不管是否旧的令牌是否过期。</param>
        /// <returns>微信API访问凭证</returns>
        public string GetToken(string appId, string appSecret, bool forceRenew = false)
        {
            //bool forceRenew = false;
            if (forceRenew)
            {
                //立即过期
                _AccessTokenBags.SetExpired(appId);
            }
            else
            {
                forceRenew = _AccessTokenBags.IsExpired(appId);
            }

            //forceRenew = CheckTokenExpire();

            if (forceRenew)
            {
                lock (_lockApiCalling)
                {
                    //再次检查，通常在等待上一锁释放期间，过期状态已经发生改变。若已改变，则不应再调用API去刷新Token！
                    if (_AccessTokenBags.IsExpired(appId))
                    {
                        AccessTokenJson json = AccessTokenApi.GetTokenAsync(appId, appSecret).Result;
                        if (json != null
                            && !string.IsNullOrEmpty(json.access_token))
                        {
                            _AccessTokenBags.Store(appId, new AccessTokenBag()
                            {
                                AccessTokenJson = json,
                                AppId = appId,
                                AppSecret = appSecret,
                                CreateTime = DateTime.Now
                            });
                        }
                    }
                }
            }

            return _AccessTokenBags.GetToken(appId);
        }
    }
}
