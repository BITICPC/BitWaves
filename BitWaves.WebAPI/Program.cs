using System;
using System.IO;
using BitWaves.Data.Repositories;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;

namespace BitWaves.WebAPI
{
    /// <summary>
    /// 为应用程序提供入口点。
    /// </summary>
    public static class Program
    {
        private static Logger _logger;

        /// <summary>
        /// 应用程序入口点。
        /// </summary>
        /// <param name="args">命令行参数。</param>
        public static void Main(string[] args)
        {
            _logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

            try
            {
                _logger.Trace("创建应用程序宿主...");
                var host = CreateWebHostBuilder(args).Build();

                InitializeDatabase(host);

                _logger.Trace("启动应用程序...");
                host.Run();
            }
            catch (Exception ex)
            {
                //NLog: catch setup errors
                _logger.Error(ex, "应用程序抛出了未经处理的异常：{0}：{1}", ex.GetType(), ex.Message);
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                LogManager.Shutdown();
            }
        }

        /// <summary>
        /// 在当前环境中查找数据库配置文件并返回找到的配置文件的路径。该方法将会尝试查找下列路径上的配置文件：
        ///
        /// * ./dbsettings.json
        /// * /etc/bitwaves/dbsettings.json
        /// </summary>
        /// <returns>找到的配置文件的路径。</returns>
        /// <exception cref="Exception">无法在任何一个路径上找到配置文件。</exception>
        private static string FindDbConfigFile()
        {
            var paths = new[] { "./dbsettings.json", "/etc/bitwaves/dbsettings.json" };
            foreach (var p in paths)
            {
                if (File.Exists(p))
                {
                    return p;
                }
            }

            throw new Exception("无法找到任何有效的数据库配置文件。");
        }

        /// <summary>
        /// 创建 <see cref="IWebHostBuilder"/> 以构建应用程序框架。
        /// </summary>
        /// <param name="args">命令行参数。</param>
        /// <returns>创建的 <see cref="IWebHostBuilder"/> 对象。</returns>
        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .ConfigureAppConfiguration((context, config) =>
                   {
                       config.AddJsonFile(FindDbConfigFile(), optional: false);
                   })
                   .ConfigureLogging(logging => { logging.ClearProviders(); })
                   .UseNLog()
                   .UseStartup<Startup>();

        /// <summary>
        /// 初始化数据库。
        /// </summary>
        /// <param name="host">已经创建的应用程序宿主环境。</param>
        private static void InitializeDatabase(IWebHost host)
        {
            _logger.Trace("初始化数据库...");

            using (var serviceScope = host.Services.CreateScope())
            {
                var repo = serviceScope.ServiceProvider.GetService<Repository>();
                var repoInitializer = new RepositoryInitializer(
                    repo, serviceScope.ServiceProvider.GetService<ILogger<RepositoryInitializer>>());

                try
                {
                    repoInitializer.Initialize();
                    repoInitializer.Seed();
                }
                catch (Exception ex)
                {
                    _logger.Error("初始化数据库失败：{0}：{1}", ex.GetType(), ex.Message);
                    throw;
                }
            }
        }
    }
}
