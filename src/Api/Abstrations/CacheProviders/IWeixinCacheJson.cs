namespace Myvas.AspNetCore.Weixin;

public interface IWeixinCacheJson
{
    int ExpiresIn { get; set; }
    bool Succeeded { get; }
}
