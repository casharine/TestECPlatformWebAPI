using Microsoft.AspNetCore;
using NLog.Web;
using System.Reflection;

namespace TestECPlatformWebAPI
{
    /// <summary>
    /// エントリーポイント
    /// </summary>
    public class Program
    {
        /// <summary>
        /// main関数
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

            try
            {
                logger.Debug("init main");
                BuildWebHost(args).Run();
            }
            catch (Exception ex)
            {
                //NLog: catch setup errors
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHost BuildWebHost(string[] args) =>
                WebHost.CreateDefaultBuilder(args)
                    .ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        var directory = Directory.GetParent(Assembly.GetExecutingAssembly().Location)!.ToString();
                        config.SetBasePath(directory);
                        config.AddJsonFile("appsettings.json");
                    })
                    .UseStartup<Startup>()
                    .ConfigureLogging(logging =>
                    {
                        logging.ClearProviders();
                        logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
                    })
                    .UseNLog()  // NLog: setup NLog for Dependency injection
                    .Build();
    }
}
