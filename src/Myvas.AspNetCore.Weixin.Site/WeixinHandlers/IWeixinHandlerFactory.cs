namespace Myvas.AspNetCore.Weixin
{
    public interface IWeixinHandlerFactory
    {
        TWeixinHandler Create<TWeixinHandler>();
    }
}
