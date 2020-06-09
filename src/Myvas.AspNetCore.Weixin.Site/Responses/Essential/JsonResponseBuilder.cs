using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin
{
    public class JsonResponseBuilder : ResponseBuilder
    {
        public JsonResponseBuilder(HttpContext context) : base(context)
        {
        }

        public override Task FlushAsync()
        {
            ContentType = ContentTypeConstants.Json;
            return base.FlushAsync();
        }
    }
}
