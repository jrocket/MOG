using MoG.Domain.Models;
using MoG.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return dbContext.Activities.Where(p => p.ProjectId == projectId);
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
    }

    public interface IActivityRepository
    {

        bool Create(Activity activity);

        IQueryable<Activity> GetByProjectId(int id);

        IQueryable<Activity> GetByUserId(int id);

        Activity GetByCommentId(int id);

        bool Delete(Activity activity);
    }
}
