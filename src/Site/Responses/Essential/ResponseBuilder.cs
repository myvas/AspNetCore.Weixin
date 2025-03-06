using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Myvas.AspNetCore.Weixin
{

    public abstract class ResponseBuilder : IResponseBuilder
    {
        public HttpContext Context { get; }
        public string ContentType { get; set; }
        public string Content { get; set; }

        public ResponseBuilder(HttpContext context)
        {
            Context = context;
        }

        public virtual async Task FlushAsync()
        {
            Context.Response.ContentType = ContentType;
            await Context.Response.WriteAsync(Content);
            Debug.WriteLine($"Response via {this.GetType().Name}:");
            Debug.WriteLine(Content);
        }
    }
}
