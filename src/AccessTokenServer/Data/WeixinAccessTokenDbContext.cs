using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin.AccessTokenServer;

public class WeixinAccessTokenDbContext : IAccessTokenDbContext
{
    public DbSet<WeixinAccessTokenEntity> AccessTokens { get; set; }

    public virtual Task<int> SaveChangesAsync();

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
