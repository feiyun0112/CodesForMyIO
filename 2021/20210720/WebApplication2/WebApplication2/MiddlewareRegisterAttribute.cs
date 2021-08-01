using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MiddlewareRegisterAttribute : Attribute
    {
        //注册顺序
        public int Sort { get; set; } = int.MaxValue;
        //生命周期
        public ServiceLifetime Lifetime { get; set; } = ServiceLifetime.Scoped;
    }

    public class MiddlewareRegisterInfo
    {
        public MiddlewareRegisterInfo(Type type,MiddlewareRegisterAttribute attribute)
        {
            Type = type;
            Sort = attribute.Sort;
            Lifetime = attribute.Lifetime;
        }
        public Type Type { get; private set; }
        public int Sort { get; private set; }
        public ServiceLifetime Lifetime { get; private set; }
    }
}
