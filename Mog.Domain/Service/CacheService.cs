using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoG.Domain.Service
{
    public class InMemoryCache : ICacheService
    {
        public T Get<T>(string cacheID, Func<T> getItemCallback) where T : class
        {
            T item = HttpRuntime.Cache.Get(cacheID) as T;
            if (item == null)
            {
                item = getItemCallback();
                HttpContext.Current.Cache.Insert(cacheID, item,null,DateTime.Now.AddSeconds(30),System.Web.Caching.Cache.NoSlidingExpiration);
            }
            return item;
        }
    }

    public interface ICacheService
    {
        T Get<T>(string cacheID, Func<T> getItemCallback) where T : class;
    }
}