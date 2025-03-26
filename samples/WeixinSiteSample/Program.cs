using System.Reflection;
using WeixinSiteSample.Data;

var assembly = typeof(Program).Assembly;
var assemblyName = assembly.GetName().Name;
var assemblyVersion = assembly.GetName().Version?.ToString()
    ?? assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
Console.WriteLine($"{assemblyName} v{assemblyVersion} starting up...");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Set default logging level.
#if DEBUG
    builder.Logging.SetMinimumLevel(LogLevel.Trace);
#else
    builder.Logging.SetMinimumLevel(LogLevel.Information);
#endif

    builder.ConfigureServices().Build().ConfigurePipeline().Run();
}
catch (Exception ex)
{
    Console.WriteLine($"{assemblyName} v{assemblyVersion} terminated for an unhandled exception occured.");
    Console.WriteLine(ex);
}
finally
{
    Console.WriteLine($"{assemblyName} v{assemblyVersion} shutdown.");
}
