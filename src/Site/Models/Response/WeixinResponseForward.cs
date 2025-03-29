using System.Collections.Generic;
using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 将来自该微信用户的上行消息转交给客服系统接管。会话最长为30分钟，超过30分钟后，即使客服没有关闭会话，本次转接会话也会自动停止。该微信用户被客服系统接管后，在会话没有超过30分钟且客服没有关闭会话前，该微信用户发送的所有上行消息均会被直接转发至客服系统。
/// </summary>
/// <remarks>
/// 若指定了客服账号，即<see cref="TransferInfo.KfAccount"/>属性不为空，则该微信用户会被分配给指定客服账号。
/// <list type="number">
/// <item>注意：仅允许指定一个客服账号），指定客服账号，可以是完整客服账号（格式为：帐号前缀@公众号微信号）或者客服人员的微信号，完整客服账号可以在公众号后台的客服管理中找到。</item>
/// <item>若指定客服账号不存在或者未绑定客服账号，则客服系统会随机分配给一个客服账号。</item>
/// <item>若指定客服账号有效，则本指定立即生效，后续该微信用户的所有上行消息均会被转接给该客服账号</item>
/// <item>若指定客服没有接入能力(不在线、没有开启自动接入或者自动接入已满)，这会导致其它客服不会收到此客户的上行消息。因此，在指定客服时，应先查询客服的接入能力（获取在线客服接待信息接口），指定到有能力接入的客服，保证客户能够及时得到服务。</item>
/// </list>
/// 
/// 若不指定客服账号，则该微信用户会被分配给系统中所有在线的客服账号。
/// <list type="number">
/// <item>如果您有多个客服人员同时登录了客服系统并且开启了自动接入，则多客服系统会自动将该客户分配给其中一个客服人员。</item>
/// <item>如果您有多个客服人员同时登录了客服系统但没有开启自动接入，则多客服系统会将该客户自动转接给其中一个客服人员，由客服人员手动接入。</item>
/// <item>如果您只有一个客服人员登录了客服系统，则该客户会直接被接入。</item>
/// <item>如果您的客服人员均未登录客服系统，则该客户会被转接至公众号后台配置的客服人员。</item>
/// </list>
/// </remarks>
/// <seealso href="https://developers.weixin.qq.com/doc/offiaccount/Customer_Service/Forwarding_of_messages_to_service_center.html">Tencent official document for Transfer received message to customer service management system</seealso>
[XmlRoot("xml", Namespace = "")]
public class WeixinResponseForward : WeixinResponse, IWeixinResponseMessage
{
    public WeixinResponseForward()
    {
        MsgType = ResponseMsgType.transfer_customer_service;
        TransInfo = new TransferInfo();
    }

    public WeixinResponseForward(string kfAccount) : this()
    {
        TransInfo ??= new TransferInfo();
        TransInfo.SetKfAccount(kfAccount);
    }

    /// <summary>
    /// 转接设置
    /// </summary>
    [XmlElement("TransInfo")]
    public TransferInfo TransInfo { get; set; }

    public override string ToXml()
    {
        return base.ToXml();
    }

    #region inner class TransferInfo
    
    public class TransferInfo
    {
        [XmlArrayItem("KfAccount")]
        public List<string> KfAccount { get; set; }

        public TransferInfo()
        {
            KfAccount = new List<string>();
        }

        /// <summary>
        /// Set only one customer service operator account.
        /// </summary>
        /// <param name="kfAccount"></param>
        /// <returns></returns>
        public bool SetKfAccount(string kfAccount)
        {
            KfAccount.Clear();
            if (!string.IsNullOrEmpty(kfAccount))
            {
                KfAccount.Add(kfAccount);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Add customer service operator accounts.
        /// </summary>
        /// <param name="kfAccounts"></param>
        /// <returns></returns>
        private bool AddKfAccounts(params string[] kfAccounts)
        {
            foreach (var kfAccount in kfAccounts)
            {
                if (!string.IsNullOrEmpty(kfAccount))
                {
                    if (!KfAccount.Contains(kfAccount))
                    {
                        KfAccount.Add(kfAccount);
                        return true;
                    }
                }
            }
            return false;
        }
    }
    #endregion
}
