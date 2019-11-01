using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AspNetCore.Weixin
{
	internal static class EventHandlerExtensions
	{
		public static void Raise<TEventArgs>(this EventHandler<TEventArgs> handler,
			object sender, TEventArgs args)
			where TEventArgs : EventArgs
		{
			handler?.Invoke(sender, args);
		}
	}
}
