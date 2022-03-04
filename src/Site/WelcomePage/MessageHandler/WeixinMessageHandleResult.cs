using System;
using System.Collections.Generic;
using System.Text;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// Contains the result of an WeixinMessageHandler.HandleRequest call
    /// </summary>
    public class WeixinMessageHandleResult
    {
        private WeixinMessageHandleResult() { }

        /// <summary>
        /// Holds failure information from the <see cref="WeixinMessageHandler"/>
        /// </summary>
        public Exception Failure { get; private set; }

        /// <summary>
        /// Indicates that stage of <see cref="WeixinMessageHandler"/> was directly handled by user intervention and no
        /// further processing should be attempted.
        /// </summary>
        public bool Handled { get; private set; }

        public static WeixinMessageHandleResult Handle()
        {
            return new WeixinMessageHandleResult() { Handled = true };
        }

        public static WeixinMessageHandleResult Fail(Exception failure)
        {
            return new WeixinMessageHandleResult() { Failure = failure };
        }

        public static WeixinMessageHandleResult Fail(string failureMessage)
        {
            return new WeixinMessageHandleResult() { Failure = new Exception(failureMessage) };
        }
    }
}
