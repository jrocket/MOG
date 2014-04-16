using MoG.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Caching;

namespace MoG.Domain.Service
{
    public class ScheduledTaskService : IScheduledTaskService
    {

        private ITempFileService serviceTempFile = null;
        private ILogService serviceLog = null;

        private static CacheItemRemovedCallback OnCacheRemove = null;
        private const string CRON_CREATE_FILE_THUMBNAIL = "CREATE_FILE_THUMBNAIL";

        /// <summary>
        /// The application cache
        /// </summary>
        private static Cache cache;

        public ScheduledTaskService(Cache _cache, ITempFileService tempService,ILogService logService)
        {
            this.serviceTempFile = tempService;
            this.serviceLog = logService;
            cache = _cache;
        }

        //public ScheduledTaskService(Cache _cache)
        //{
        //    cache = _cache;
        //}


        /// <summary>
        /// Starts the service.
        /// </summary>
        public void StartService()
        {
            StartService(30);//86400 seconds = 24 hours
        }

        public void StartService(int seconds)
        {
            if (cache[CRON_CREATE_FILE_THUMBNAIL] != null) return;

            AddTask(CRON_CREATE_FILE_THUMBNAIL, seconds);

        }

        public DateTime? GetThumbnailNextRun()
        {
            return GetCacheUtcExpiryDateTime(CRON_CREATE_FILE_THUMBNAIL);
        }

        private DateTime? GetCacheUtcExpiryDateTime(string cacheKey)
        {
            object cacheEntry = cache.GetType().GetMethod("Get", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(cache, new object[] { cacheKey, 1 });
            if (cacheEntry != null)
            {
                PropertyInfo utcExpiresProperty = cacheEntry.GetType().GetProperty("UtcExpires", BindingFlags.NonPublic | BindingFlags.Instance);
                DateTime utcExpiresValue = (DateTime)utcExpiresProperty.GetValue(cacheEntry, null);

                return utcExpiresValue.ToLocalTime();
            }
            return null;
        }


        private void AddTask(string name, int seconds)
        {
            OnCacheRemove = new CacheItemRemovedCallback(CacheItemRemoved);
            cache.Insert(name, seconds, null,
                DateTime.Now.AddSeconds(seconds), Cache.NoSlidingExpiration,
                CacheItemPriority.NotRemovable, OnCacheRemove);
        }

        private void CacheItemRemoved(string key, object value, CacheItemRemovedReason r)
        {
            if (r == CacheItemRemovedReason.Removed)
                return;
            switch (key)
            {
                case CRON_CREATE_FILE_THUMBNAIL:
                    TaskCreateFileThumbnail();
                    break;

            }
            // do stuff here if it matches our taskname, like WebRequest
            // re-add our task so it recurs
            AddTask(key, Convert.ToInt32(value));
        }



        /// <summary>
        /// Stops the service.
        /// </summary>
        public void StopService()
        {
            cache.Remove(CRON_CREATE_FILE_THUMBNAIL);
        }

        /// <summary>
        /// Gets a value indicating whether [is service running].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is service running]; otherwise, <c>false</c>.
        /// </value>
        public bool IsServiceRunning
        {
            get
            {
                return cache[CRON_CREATE_FILE_THUMBNAIL] != null;
            }
        }


        private void TaskCreateFileThumbnail()
        {
            MoG.Domain.Models.TempUploadedFile nextFile = serviceTempFile.GetNextInQueue();
            if (nextFile != null)
            {
               bool result = false;
                try
                {
                   result =  this.serviceTempFile.Process(nextFile);
                }
                catch (Exception exc)
                {
                    this.serviceLog.LogError("TaskCreateFileThumbnail",exc);
                    result = false;
                }
                if (!result)
                {
                    this.serviceTempFile.MoveToErrorQueue(nextFile);
                }
            }
      
            if (serviceTempFile.GetQueueLength()>0)
            {
                TaskCreateFileThumbnail();
            }
        }
    }

    public interface IScheduledTaskService
    {
         void StartService();
    }
}