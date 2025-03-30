using Microsoft.Extensions.Configuration;

namespace Myvas.AspNetCore.Weixin.Api.RealTests;

public abstract class RealRedisServerBase
{
    /// <summary>
    /// The configuration provider.
    /// </summary>
    /// <remarks>
    /// <para>
    /// For GitHub secrets, or environment variables:
    /// The key name has a rule:
    /// <list type="number">
    /// <item>Upper all characters.</item>
    /// <item>Replace colon with double underscore.</item>
    /// </list>
    /// e.g. a key "Weixin:AppId" (in `usersecrets` and `appsettings.json`) should be 
    /// converted to "WEIXIN__APPID" (in GitHub secrets or environment variables).
    /// </para>
    /// </remarks>
    protected IConfiguration Configuration { get; }

    /// <summary>
    /// Whether enable to test on a real redis server (optional, default is false)
    /// </summary>
    /// <remarks>
    /// <para>For usersecrets, or appsettings.json:
    /// The key is `Weixin:EnableRealRedisTests`.</para><para>
    /// For GitHub secrets, or environment variables:
    /// The key is `WEIXIN__ENABLEREALREDISTESTS`.</para>
    /// </remarks>
    protected bool EnableRealRedisTests { get; }

    /// <summary>
    /// The connection string to the redis server (optional, default is "localhost")
    /// </summary>
    /// <remarks>
    /// <para>For usersecrets, or appsettings.json:
    /// The key is `ConnectionStrings:RedisConnection`.</para><para>
    /// For GitHub secrets, or environment variables:
    /// The key is `CONNECTIONSTRINGS__REDISCONNECTION`.</para>
    /// </remarks>
    protected string RedisConnectionString { get; }

    public RealRedisServerBase()
    {
        Configuration = new ConfigurationBuilder()
            .AddUserSecrets("Myvas.AspNetCore.Weixin.Tests")  // The UserSecretsId specified by this xunit test project.
            .AddEnvironmentVariables()
            .Build();

        EnableRealRedisTests = Configuration.GetValue<bool>("Weixin:EnableRealRedisTests", false);

        if (EnableRealRedisTests)
        {
            RedisConnectionString = Configuration.GetValue("ConnectionStrings:RedisConnection", "localhost");
        }
    }
}
