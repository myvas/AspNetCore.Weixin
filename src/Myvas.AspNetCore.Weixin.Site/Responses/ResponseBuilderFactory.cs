using Microsoft.AspNetCore.Http;
using System;

namespace Myvas.AspNetCore.Weixin
{
    public class ResponseBuilderFactory : IResponseBuilderFactory
    {
        public TResponseBuilder Create<TResponseBuilder>(HttpContext requestContext)
            where TResponseBuilder : ResponseBuilder
        {
            return (TResponseBuilder)Activator.CreateInstance(typeof(TResponseBuilder), new object[] { requestContext });
        }
    }
}
