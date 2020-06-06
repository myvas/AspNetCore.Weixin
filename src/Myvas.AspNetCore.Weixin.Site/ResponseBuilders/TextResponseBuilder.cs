using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin
{
    public class TextResponseBuilder : WeixinResponseBuilder<WeixinResponseText>
    {
        public TextResponseBuilder(HttpContext context) : base(context)
        {
        }


        public void SetText(string text)
        {
            if (Response == null) return;

            Response.Content = text;
        }

        public override Task FlushAsync()
        {
            Content = Response.ToString();
            return base.FlushAsync();
        }
    }

}
