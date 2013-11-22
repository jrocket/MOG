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
    }

    public interface IActivityRepository
    {

        bool Create(Activity activity);

        IQueryable<Activity> GetByProjectId(int id);
    }
}
