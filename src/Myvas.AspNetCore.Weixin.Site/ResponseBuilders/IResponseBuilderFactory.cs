using Microsoft.AspNetCore.Http;

namespace Myvas.AspNetCore.Weixin
{
    public interface IResponseBuilderFactory
    {
        TResponseBuilder Create<TResponseBuilder>(HttpContext requestContext) where TResponseBuilder : ResponseBuilder;
    }
}