using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using AspNetCore.Weixin;

namespace Demo
{
    public class Startup
    {
        private readonly IConfigurationRoot _configuration;
        private readonly ILogger<Startup> _logger;

        public Startup(IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory?.CreateLogger<Startup>() ?? throw new ArgumentNullException(nameof(loggerFactory));

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            _configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var weixinOptions = _configuration.GetSection("Weixin");
            services.AddWeixinAccessToken(options =>
            {
                options.AppId = weixinOptions["AppId"];
                options.AppSecret = weixinOptions["AppSecret"];
            });
            services.AddWeixinWelcomePage(options =>
            {
                options.AppId = weixinOptions["AppId"];
                options.AppSecret = weixinOptions["AppSecret"];
                options.WebsiteToken = weixinOptions["WebsiteToken"];
                options.EncodingAESKey = weixinOptions["EncodingAESKey"];
                options.Events = new WeixinMessageEvents()
                {
                    OnTextMessageReceived = ctx =>
                    {
                        var sender = ctx.Sender;
                        var e = ctx.Args;

                        _logger.LogInformation($"OnTextMessageRecived: {e.Content}");
                        return true;
                    },
                    OnLinkMessageReceived = ctx =>
                    {
                        var sender = ctx.Sender;
                        var e = ctx.Args;

                        _logger.LogInformation($"OnLinkMessageReceived: {ctx.Args.Url}");

                        var messageHandler = sender;
                        var responseMessage = messageHandler.CreateResponseMessage<ResponseMessageText>();
                        responseMessage.Content = string.Format(@"您发送了一条连接信息：
            Title：{0}
            Description:{1}
            Url:{2}", e.Title, e.Description, e.Url);
                        messageHandler.ResponseMessage = responseMessage;

                        return true;
                    }
                };
            });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            
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