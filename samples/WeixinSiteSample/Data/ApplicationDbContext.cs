using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.DbContexts;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.Interfaces;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.Options;

namespace WeixinSiteSample.Data
{
    public class ApplicationDbContext : PersistedTokenDbContext<ApplicationDbContext>, IPersistedTokenDbContext//<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
