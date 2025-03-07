namespace Myvas.AspNetCore.Weixin.Stores.Serialization;

#pragma warning disable 1591

public class ClaimsPrincipalLite
{
    public string AuthenticationType { get; set; }
    public ClaimLite[] Claims { get; set; }
}