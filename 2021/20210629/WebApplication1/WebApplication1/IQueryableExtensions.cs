using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebApplication1
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Where<T>(
            this IQueryable<T> query,
            bool condition,
             Expression<Func<T, bool>> predicate)
        {
            return condition
                ? query.Where(predicate)
                : query;
        }
    }
}
