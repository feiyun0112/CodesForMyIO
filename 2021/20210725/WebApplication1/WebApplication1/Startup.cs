using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Polly;
using Polly.Contrib.Simmy;
using Polly.Contrib.Simmy.Latency;
using Polly.Contrib.Simmy.Outcomes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WebApplication1
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApplication1", Version = "v1" });
            });

            IAsyncPolicy<HttpResponseMessage> chaosPolicy = MonkeyPolicy.InjectLatencyAsync<HttpResponseMessage>(with =>
                with.Latency(TimeSpan.FromSeconds(5))
                    .InjectionRate(.5)
                    //Ä¬ÈÏ¹Ø±Õ
                    .EnabledWhen(GetChaosEnabled)
                );

            services.AddHttpClient("Weather", client =>
            {
                client.BaseAddress = new Uri("http://www.weather.com.cn/");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.Timeout = TimeSpan.FromSeconds(2);
            }).AddPolicyHandler(chaosPolicy);
        }

        public static bool ChaosEnabled = false;
        private static Task<bool> GetChaosEnabled(Context context, CancellationToken ct)
        {
            return Task.FromResult(ChaosEnabled);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApplication1 v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
