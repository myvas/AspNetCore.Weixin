using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public interface IWeixinCustomerSupportApi
{
    Task<WeixinErrorJson> SendImage(string openId, string mediaId);
    Task<WeixinErrorJson> SendMusic(string openId, string title, string description, string musicUrl, string hqMusicUrl, string thumbMediaId);
    Task<WeixinErrorJson> SendNews(WeixinCustomerServiceMessageNews news, CancellationToken cancellationToken = default);
    Task<WeixinErrorJson> SendNews(string destOpenId, IList<WeixinCustomerServiceMessageNewsArticle> articles, CancellationToken cancellationToken = default);
    Task<WeixinErrorJson> SendText(string openId, string content);
    Task<WeixinErrorJson> SendVideo(string openId, string mediaId, string thumbMediaId);
    Task<WeixinErrorJson> SendVoice(string openId, string mediaId);
}
