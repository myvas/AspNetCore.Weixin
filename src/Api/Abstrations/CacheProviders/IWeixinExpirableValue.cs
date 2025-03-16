namespace Myvas.AspNetCore.Weixin;

public interface IWeixinExpirableValue
{
    string Value { get; set; }
    int ExpiresIn { get; set; }
    bool Validate();
}
