using Microsoft.EntityFrameworkCore;

namespace Myvas.AspNetCore.Weixin.AccessTokenServer;

public interface IAccessTokenDbContext : IDisposable
{
    DbSet<WeixinAccessTokenEntity> AccessTokens { get; set; }
}
