namespace Myvas.AspNetCore.Weixin
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
			WeixinMessageHandler sender,
			TEventArgs args,
			bool needEncrypt)
		{
			Sender = sender;
			Args = args;
			NeedEncrypt = needEncrypt;
		}

		/// <summary>
		/// Gets the sender
		/// </summary>
		public WeixinMessageHandler Sender { get; }

		/// <summary>
		/// Gets the event args
		/// </summary>
		public TEventArgs Args { get; }

		/// <summary>
		/// Gets whether WeixinMessageEncryptor is must.
		/// </summary>
		public bool NeedEncrypt { get; }
	}
}
