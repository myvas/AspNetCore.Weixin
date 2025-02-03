using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin
{
    public interface IWeixinHandler
    {
        HttpContext Context { get; set; }
        string Text { get; set; }
        Task<bool> HandleAsync();
    }

    public interface IWeixinHandler<T> : IWeixinHandler
        where T : ReceivedXml
    {
        T Xml { get; set; }
    }
}
