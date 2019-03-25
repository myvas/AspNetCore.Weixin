using Myvas.AspNetCore.TencentSms;
using AspNetCore.Weixin;
using Demo.Applications;
using Demo.Data;
using Demo.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;

namespace Demo
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.Configure<CookiePolicyOptions>(options =>
			{
				// This lambda determines whether user consent for non-essential cookies is needed for a given request.
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});

			services.AddDbContext<IdentityDbContext>(options => options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
			services.AddDbContext<WeixinDbContext>(options => options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

			services.AddIdentity<AppUser, IdentityRole>()
				.AddEntityFrameworkStores<IdentityDbContext>()
				.AddUserManager<AppUserManager>()
				.AddSignInManager<SignInManager<AppUser>>()
				.AddDefaultTokenProviders();
			services.Configure<IdentityOptions>(options =>
			{
				options.Password = new PasswordOptions
				{
					RequireLowercase = false,
					RequireUppercase = false,
					RequireNonAlphanumeric = false,
					RequireDigit = false
				};
				options.User.RequireUniqueEmail = false;
				options.SignIn.RequireConfirmedEmail = false;

				options.SignIn.RequireConfirmedPhoneNumber = true;
			});
			services.ConfigureApplicationCookie(options =>
			{
				options.LoginPath = "/Account/Login";
				options.LogoutPath = "/Account/LogOff";
				options.AccessDeniedPath = "/Account/AccessDenied";
			});

			services.AddAuthentication()
				.AddWeixinOpen(options =>
				{
					options.AppId = Configuration["WeixinOpen:AppId"];
					options.AppSecret = Configuration["WeixinOpen:AppSecret"];
				})
				.AddWeixinAuth(options =>
				{
					options.AppId = Configuration["WeixinAuth:AppId"];
					options.AppSecret = Configuration["WeixinAuth:AppSecret"];
				});
			services.AddTencentSms(options =>
			{
				options.SdkAppId = Configuration["QcloudSms:SdkAppId"];
				options.AppKey = Configuration["QcloudSms:AppKey"];
			});
			services.AddViewDivert();

			services.AddWeixinAccessToken(options =>
			{
				options.AppId = Configuration["Weixin:AppId"];
				options.AppSecret = Configuration["Weixin:AppSecret"];
			});
			services.AddWeixinJssdk(options =>
			{
				options.AppId = Configuration["Weixin:AppId"];
			});
			services.AddScoped<IWeixinEventSink, WeixinEventSink>();
			var weixinEventSink = services.BuildServiceProvider().GetRequiredService<IWeixinEventSink>();
			services.AddWeixinWelcomePage(options =>
			{
				options.AppId = Configuration["Weixin:AppId"];
				options.AppSecret = Configuration["Weixin:AppSecret"];
				options.WebsiteToken = Configuration["Weixin:WebsiteToken"];
				options.EncodingAESKey = Configuration["Weixin:EncodingAESKey"];
				options.Path = "/wx";
				options.Events = new WeixinMessageEvents()
				{
					OnTextMessageReceived = ctx => weixinEventSink.OnTextMessageReceived(ctx.Sender, ctx.Args),
					OnLinkMessageReceived = ctx => weixinEventSink.OnLinkMessageReceived(ctx.Sender, ctx.Args),
					OnClickMenuEventReceived = ctx => weixinEventSink.OnClickMenuEventReceived(ctx.Sender, ctx.Args),
					OnImageMessageReceived = ctx => weixinEventSink.OnImageMessageReceived(ctx.Sender, ctx.Args),
					OnLocationEventReceived = ctx => weixinEventSink.OnLocationEventReceived(ctx.Sender, ctx.Args),
					OnLocationMessageReceived = ctx => weixinEventSink.OnLocationMessageReceived(ctx.Sender, ctx.Args),
					OnQrscanEventReceived = ctx => weixinEventSink.OnQrscanEventReceived(ctx.Sender, ctx.Args),
					OnEnterEventReceived = ctx => weixinEventSink.OnEnterEventReceived(ctx.Sender, ctx.Args),
					OnSubscribeEventReceived = ctx => weixinEventSink.OnSubscribeEventReceived(ctx.Sender, ctx.Args),
					OnUnsubscribeEventReceived = ctx => weixinEventSink.OnUnsubscribeEventReceived(ctx.Sender, ctx.Args),
					OnVideoMessageReceived = ctx => weixinEventSink.OnVideoMessageReceived(ctx.Sender, ctx.Args),
					OnShortVideoMessageReceived = ctx => weixinEventSink.OnShortVideoMessageReceived(ctx.Sender, ctx.Args),
					OnViewMenuEventReceived = ctx => weixinEventSink.OnViewMenuEventReceived(ctx.Sender, ctx.Args),
					OnVoiceMessageReceived = ctx => weixinEventSink.OnVoiceMessageReceived(ctx.Sender, ctx.Args)
				};
			});

			services.AddMvc()
				.SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_1);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				//app.UseHsts();
			}

			//app.UseHttpsRedirection();
			app.UseStaticFiles();
			//app.UseCookiePolicy();

			app.UseAuthentication();

			app.UseWeixinWelcomePage();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}