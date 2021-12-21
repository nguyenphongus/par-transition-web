using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Reflection;

namespace par_transition_web.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var hostEnvironment = hostingContext.HostingEnvironment;
                    config.SetBasePath(hostEnvironment.ContentRootPath);

                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                          .AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}.json", true, true);

                    var appAssembly = Assembly.Load(new AssemblyName(hostEnvironment.ApplicationName));
                    if (appAssembly != null)
                        config.AddUserSecrets(appAssembly, optional: true);

                    config.AddEnvironmentVariables();
                    if (args != null)
                        config.AddCommandLine(args);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStaticWebAssets();
                    webBuilder.UseStartup<Startup>();
                });
    }
}
