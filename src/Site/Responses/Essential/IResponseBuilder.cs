using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public interface IResponseBuilder
{
    HttpContext Context { get; }
    string ContentType { get; set; }
    string Content { get; set; }
    Task FlushAsync();
}