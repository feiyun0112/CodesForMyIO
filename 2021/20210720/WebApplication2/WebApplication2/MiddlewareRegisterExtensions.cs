using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace WebApplication2
{
    public static class MiddlewareRegisterExtensions
    {
        private static readonly IEnumerable<MiddlewareRegisterInfo> _middlewareRegisterInfos = GetMiddlewareRegisterInfos();
        public static IServiceCollection AddMiddlewares(this IServiceCollection services)
        {
            foreach (var middlewareRegisterInfo in _middlewareRegisterInfos)
            {
                switch (middlewareRegisterInfo.Lifetime)
                {
                    case ServiceLifetime.Singleton:
                        services.AddSingleton(middlewareRegisterInfo.Type);
                        break;
                    case ServiceLifetime.Transient:
                        services.AddTransient(middlewareRegisterInfo.Type);
                        break;
                    default:
                        services.AddScoped(middlewareRegisterInfo.Type);
                        break;
                }
            }

            return services;
        }
        public static IApplicationBuilder UseMiddlewares(this IApplicationBuilder applicationBuilder)
        {
            foreach (var middlewareRegisterInfo in _middlewareRegisterInfos)
            {
                applicationBuilder.UseMiddleware(middlewareRegisterInfo.Type);
            }

            return applicationBuilder;
        }

        private static List<MiddlewareRegisterInfo> GetMiddlewareRegisterInfos()
        {
            var middlewareRegisterInfos = new List<MiddlewareRegisterInfo>();
            //所有包含Middleware的Assembly
            var assemblies = new Assembly[] { typeof(Startup).Assembly };
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes().Where(x => !x.IsAbstract))
                {
                    var attribute = type.GetCustomAttribute<MiddlewareRegisterAttribute>();

                    if (attribute != null)
                    {
                        middlewareRegisterInfos.Add(new MiddlewareRegisterInfo(type, attribute));
                    }
                }
            }

            return middlewareRegisterInfos.OrderBy(p=>p.Sort).ToList();
        }
    }
}
