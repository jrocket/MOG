using MoG.Domain.Models;
using MoG.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace MoG.Domain.Repository
{
    public class ActivityRepository : BaseRepository , IActivityRepository
    {
      
        public ActivityRepository(IdbContextProvider provider) : base(provider)
        {
          
        }


        public bool Create(Activity activity)
        {
            dbContext.Activities.Add(activity);
            int result = dbContext.SaveChanges();
            return (result > 0);
        }



        public IQueryable<Activity> GetByProjectId(int projectId)
        {
            List<int> ids = new List<int>();
            ids.Add(projectId);
            return this.GetNotificationsByProjectIds(ids,null);
        }


        public IQueryable<Activity> GetByUserId(int id)
        {
            return dbContext.Activities
                .Where(p => p.Who.Id == id)
                .OrderByDescending(p => p.When);
        }


        public Activity GetByCommentId(int id)
        {
            return dbContext.Activities
               .Where(p => p.CommentId == id)
               .FirstOrDefault();
        }

        public bool Delete(Activity activity)
        {
            if (activity== null)
                return false;
            this.dbContext.Activities.Remove(activity);
            int result = dbContext.SaveChanges();
            return result > 0;
        }





        public IQueryable<Activity> GetNotificationsByProjectIds(List<int> projectIds, int? excludedUserId)
        {
            var query =  this.dbContext.Activities
                 .Include(c => c.Project)
                 .Include(a => a.File)
                 .Include(a => a.Who)
                .Where(a => (a.ProjectId.HasValue ? projectIds.Contains(a.ProjectId.Value) : false) 
                || (a.File !=null ? projectIds.Contains( a.File.ProjectId) : false)
                );
            if (excludedUserId != null)
            {
                query = query.Where(a => a.Who.Id != excludedUserId.Value);
            }

            query = query.OrderByDescending(a => a.When);

            return query;
        }
    }

    public interface IActivityRepository
    {

        bool Create(Activity activity);

        IQueryable<Activity> GetByProjectId(int id);

        IQueryable<Activity> GetByUserId(int id);

        Activity GetByCommentId(int id);

        bool Delete(Activity activity);


        IQueryable<Activity> GetNotificationsByProjectIds(List<int> projectIds, int? excludedUserId);
    }
}
