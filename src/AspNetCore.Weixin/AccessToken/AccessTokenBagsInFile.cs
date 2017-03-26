using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Myvas.AspNetCore.Weixin
{
    public class AccessTokenBagsInFile : IAccessTokenBagsRepository
    {
        /// <summary>
        /// AccessToken管理类。在文件中缓存AccessToken。
        /// <para>将AccessToken存储在文件中，因此使用本类的微信应用将支持跨进程、跨Web站点部署，也可支持跨服务器部署（通过文件共享方式，请注意访问权限问题）。</para>
        /// </summary>
        private static readonly object _lockBagInitializing = new object();

        /// <summary>
        /// 凭证文件所在文件夹
        /// </summary>
        private string _FolderPath;

        /// <summary>
        /// 注册持久化接口
        /// </summary>
        /// <param name="filePath"></param>
        public AccessTokenBagsInFile(string folerPath)
        {
            _FolderPath = folerPath;
        }

        public AccessTokenBag Load(string appId)
        {
            AccessTokenBag bag = new AccessTokenBag();
            try
            {
                string filePath = Path.Combine(_FolderPath, appId);
                using (var fs = new FileStream(filePath, FileMode.Open))
                using (var sr = new StreamReader(fs))
                {
                    bag.AccessTokenJson.access_token = sr.ReadLine();
                    bag.AccessTokenJson.expires_in = int.Parse(sr.ReadLine());
                    bag.CreateTime = DateTime.Parse(sr.ReadLine());
                }
            }
            catch
            {
            }
            return bag;
        }

        public void Store(string appId, AccessTokenBag bag)
        {
            try
            {
                string filePath = Path.Combine(_FolderPath, appId);
                using (var fs = new FileStream(filePath, FileMode.OpenOrCreate))
                using (var sw = new StreamWriter(fs))
                {
                    sw.WriteLine(bag.AccessTokenJson.access_token);
                    sw.WriteLine(bag.AccessTokenJson.expires_in);
                    sw.WriteLine(bag.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                }
            }
            catch
            {
            }
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