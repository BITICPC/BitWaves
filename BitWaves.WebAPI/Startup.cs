﻿using BitWaves.Data.DependencyInjection;
using BitWaves.WebAPI.Middlewares;
using BitWaves.WebAPI.Services.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BitWaves.WebAPI
{
    /// <summary>
    /// 为应用程序提供初始化逻辑。
    /// </summary>
    public sealed class Startup
    {
        private readonly IConfiguration _config;
        private readonly ILogger<Startup> _logger;

        /// <summary>
        /// 初始化 <see cref="Startup"/> 类的新实例。
        /// </summary>
        /// <param name="config">应用程序配置目录。</param>
        /// <param name="logger">日志组件。</param>
        public Startup(IConfiguration config, ILogger<Startup> logger)
        {
            _config = config;
            _logger = logger;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // 添加 BitWaves 数据仓库
            var connectionString = _config.GetConnectionString("mongodb");
            _logger.LogDebug("找到 MongoDB 连接字符串：\"{0}\"", connectionString);
            services.AddBitWavesRepository(connectionString);

            // 添加不带签名和加密的 JWT 服务
            services.AddPlainJoseJwt();
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
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBitWavesAuthentication();
            app.UseMvc();
        }
    }
}
