using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin
{

    public class NewsResponseBuilder : WeixinResponseBuilder<WeixinResponseNews>
    {
        public NewsResponseBuilder(HttpContext context) : base(context)
        {
        }

        public void AddArticle(Article article)
        {
            if (Response == null) return;

            if (Response.Articles == null) Response.Articles = new List<Article>();

            Response.Articles.Add(article);
        }

        public override Task FlushAsync()
        {
            Content = Response.ToString();
            return base.FlushAsync();
        }
    }
}
