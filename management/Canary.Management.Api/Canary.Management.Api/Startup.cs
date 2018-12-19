using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Canary.HaProxy;
using Canary.Kubernetes;
using k8s;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rocket.Engine.Contracts;
using Rocket.Engine.Services;

namespace Canary.Management.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //string kubernetesHost = "http://127.0.0.1:6445";
            services.AddSingleton<ICommandExecutor, CommandExecutor>();
            services.AddSingleton<IQueryExecutor, QueryExecutor>();
            var config = KubernetesClientConfiguration.BuildConfigFromConfigFile(new FileInfo(@"C:\Users\Joe\.kube\config-dev"));
            services.AddSingleton<IKubernetesActions>(provider =>
                new KubernetesActions(config, provider.GetService<IQueryExecutor>(),
                    provider.GetService<ICommandExecutor>()));
            services.AddSingleton<IHaProxyActions>(provider =>
                new HaProxyActions("127.0.0.1:9999", provider.GetService<IQueryExecutor>(),
                    provider.GetService<ICommandExecutor>()));
            services.AddSingleton<IManagementActions, ManagementActions>();
            services.AddDistributedMemoryCache();
            services.AddSingleton<ICachingService, CachingService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                });
            }

            app.UseMvc();
        }
    }
}
