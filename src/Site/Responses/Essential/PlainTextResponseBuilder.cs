using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin
{
    public class PlainTextResponseBuilder : ResponseBuilder
    {
        public PlainTextResponseBuilder(HttpContext context) : base(context)
        {
        }

        public override Task FlushAsync()
        {
            ContentType = ContentTypeConstants.PlainText;
            return base.FlushAsync();
        }
    }
}
