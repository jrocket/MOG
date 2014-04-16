using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MoG.Domain.Common
{
    class CacheHelper
    {
        public static T GetCached<T>(string key, Func<T> initializer, TimeSpan slidingExpiration, DateTime absoluteExpiration)
        {
            var httpContext = HttpContext.Current;

            if (httpContext != null)
            {
                key = string.Intern(key);
                lock (key) // locking on interned key
                {
                    var obj = httpContext.Cache[key];
                    if (obj == null)
                    {
                        obj = initializer();
                        httpContext.Cache.Add(key, obj, null, absoluteExpiration, slidingExpiration, System.Web.Caching.CacheItemPriority.Default, null);
                    }
                    // taking care of value types
                    if (obj == null && (typeof(T)).IsValueType)
                    {
                        return default(T);
                    }
                    return (T)obj;
                }
            }
            else
            {
                return initializer(); // no available cache
            }
        }
    }
}
