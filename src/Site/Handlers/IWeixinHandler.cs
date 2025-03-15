using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public interface IWeixinSite
{
    WeixinContext Context{get;set;}

    /// <summary>
    /// Parse the property <see cref="Text"/>, and flush a response.
    /// </summary>
    /// <returns>Always return true.</returns>
    Task<bool> HandleAsync();
}

public interface IWeixinSite<T> : IWeixinSite
    where T : ReceivedXml
{
    /// <summary>
    /// The <see cref="ReceivedXml"/> parsed from the property <see cref="Text"/>.
    /// </summary>
    T Xml { get; set; }
}
