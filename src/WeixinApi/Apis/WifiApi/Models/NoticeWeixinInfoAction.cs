using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// Wi-Fi服务提供商通知微信触发验证通知，或Wi-Fi服务提供商通知微信用户登录结果
    /// </summary>
    public enum NoticeWeixinInfoAction
    {
        /// <summary>
        /// 当用户未经扫一扫而手动连接Wi-Fi，或者用户之前设备记忆自动连接Wi-Fi后，发起网络请求不通过，Wi-Fi服务提供商通知微信触发验证通知时调用。
        /// </summary>
        push,

        /// <summary>
        /// 当用户成功连接登录授权AP网络时，Wi-Fi服务提供商通知微信用户登录结果。
        /// </summary>
        loginSuccess,
        /// <summary>
        /// 当拒绝用户连接登录授权AP网络时，Wi-Fi服务提供商通知微信用户登录结果。
        /// </summary>
        loginFail
    }
}
