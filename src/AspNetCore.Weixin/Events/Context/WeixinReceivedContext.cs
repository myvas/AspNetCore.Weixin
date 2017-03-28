using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Text;

namespace AspNetCore.Weixin
{
    /// <summary>
    /// Contains information about the login session as well as the user <see cref="System.Security.Claims.ClaimsIdentity"/>.
    /// </summary>
    public class WeixinReceivedContext<TEventArgs> //: BaseWeixinContext
        where TEventArgs : ReceivedEventArgs
    {
        /// <summary>
        /// Initializes a <see cref="WeixinMessageEventReceivedContext"/>
        /// </summary>
        /// <param name="context">The HTTP environment</param>
        /// <param name="options">The options for Weixin</param>
        /// <param name="sender">The sender respective for <see cref="MessageHandler{TC}}"/></param>
        /// <param name="args"><see cref="TEventArgs"/></param>
        public WeixinReceivedContext(
            MessageHandler<MessageContext> sender,
            TEventArgs args)
        {
            Sender = sender;
            Args = args;
        }

        /// <summary>
        /// Gets the sender
        /// </summary>
        public MessageHandler<MessageContext> Sender { get; }

        /// <summary>
        /// Gets the event args
        /// </summary>
        public TEventArgs Args { get; }
    }
}
