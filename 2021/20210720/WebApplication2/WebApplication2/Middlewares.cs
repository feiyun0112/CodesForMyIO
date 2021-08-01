using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2
{

    [MiddlewareRegister(Sort = 100)]
    public class OneMiddleware : IMiddleware
    {
        private readonly ILogger<OneMiddleware> logger;

        public OneMiddleware(ILogger<OneMiddleware> logger)
        {
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            logger.LogInformation("One");
            await next(context);
        }
    }

    [MiddlewareRegister(Sort = 200)]
    public class TwoMiddleware : IMiddleware
    {
        private readonly ILogger<TwoMiddleware> logger;

        public TwoMiddleware(ILogger<TwoMiddleware> logger)
        {
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            logger.LogInformation("Two");
            await next(context);
        }
    }

    [MiddlewareRegister(Sort = 150)]
    public class ThreeMiddleware : IMiddleware
    {
        private readonly ILogger<ThreeMiddleware> logger;

        public ThreeMiddleware(ILogger<ThreeMiddleware> logger)
        {
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            logger.LogInformation("Three");
            await next(context);
        }
    }
}
