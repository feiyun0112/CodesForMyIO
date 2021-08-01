using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication3
{
    public class IdempotentAttributeFilter : IActionFilter, IResultFilter
    {
        private readonly IDistributedCache _distributedCache;
        private bool _isIdempotencyCache= false;
        const string IdempotencyKeyHeaderName = "IdempotencyKey";
        private string _idempotencyKey;
        public IdempotentAttributeFilter(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Microsoft.Extensions.Primitives.StringValues idempotencyKeys;
            context.HttpContext.Request.Headers.TryGetValue(IdempotencyKeyHeaderName, out idempotencyKeys);
            _idempotencyKey = idempotencyKeys.ToString();

            var cacheData = _distributedCache.GetString(GetDistributedCacheKey());
            if (cacheData != null)
            {
                context.Result = JsonConvert.DeserializeObject<ObjectResult>(cacheData);
                _isIdempotencyCache = true;
                return;
            }
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            //已缓存
            if (_isIdempotencyCache)
            {
                return;
            }

            var contextResult = context.Result;
            
            DistributedCacheEntryOptions cacheOptions = new DistributedCacheEntryOptions();
            cacheOptions.AbsoluteExpirationRelativeToNow = new TimeSpan(24, 0, 0);

            //缓存:
            _distributedCache.SetString(GetDistributedCacheKey(), JsonConvert.SerializeObject(contextResult), cacheOptions);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
        }

        private string GetDistributedCacheKey()
        {
            return "Idempotency:" + _idempotencyKey;
        }
    }
}
