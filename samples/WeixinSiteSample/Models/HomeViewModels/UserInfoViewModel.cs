using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace WeixinSiteSample.Models;

public class UserInfoViewModel
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public string? TokenType { get; set; }
    public string? ExpiresAt { get; set; }
    public ClaimsPrincipal? User { get; set; }
    public ExternalLoginInfo? ExternalLoginInfo { get; set; }
}
