using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public interface IWeixinHandler
{
    /// <summary>
    /// The <see cref="HttpContext"/> from request.
    /// </summary>
    HttpContext Context { get; set; }

    /// <summary>
    /// The string parsed from the request body of <see cref="Context"/>.
    /// </summary>
    string Text { get; set; }

    /// <summary>
    /// Parse the property <see cref="Text"/>, and flush a response.
    /// </summary>
    /// <returns>Always return true.</returns>
    Task<bool> HandleAsync();
}

public interface IWeixinHandler<T> : IWeixinHandler
    where T : ReceivedXml
{
    /// <summary>
    /// The <see cref="ReceivedXml"/> parsed from the property <see cref="Text"/>.
    /// </summary>
    T Xml { get; set; }
}
