using MoG.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace MoG.Domain.Repository
{
    public class FollowRepository : BaseRepository, IFollowRepository
    {

        public FollowRepository(IdbContextProvider provider)
            : base(provider)
        {

        }






        public IQueryable<Follow> Get(int userId)
        {
            return this.dbContext.Follows
               .Where(i => i.FollowerId == userId);

        }

        public int Create(Follow follow)
        {
            follow.CreatedOn = DateTime.Now;
            this.dbContext.Follows.Add(follow);
            this.dbContext.SaveChanges();
            return follow.Id;
        }




        public bool SaveChanges(Follow follow)
        {
            this.dbContext.Entry(follow).State = System.Data.Entity.EntityState.Modified;
            int result = this.dbContext.SaveChanges();
            return result > 0;
        }



        public bool IsFollowed(int projectId, int userId)
        {
            return this.dbContext.Follows.
                Where(i => i.ProjectId == projectId && i.FollowerId == userId)
                .Count() > 0;
        }

        public IQueryable<Follow> GetByUser(int userId)
        {
            this.dbContext.Configuration.ProxyCreationEnabled = false;

            return this.dbContext.Follows
                .Include(f => f.Follower)
                .Include(f => f.Project)
             .Where(i => i.FollowerId == userId);
        }

        public IQueryable<Follow> GetByProject(int ProjectId)
        {
            this.dbContext.Configuration.ProxyCreationEnabled = false;
            return this.dbContext.Follows
                .Include(f => f.Follower)
              .Where(i => i.ProjectId == ProjectId);
        }

        public Follow GetById(int id)
        {
            return this.dbContext.Follows.Find(id);
        }

        public Follow Get(int projectId, int userId)
        {
            return this.dbContext.Follows
                .Where(i => i.FollowerId == userId && i.ProjectId == projectId).FirstOrDefault();
        }




        public bool Delete(Follow follow)
        {

            if (follow != null)
            {
                this.dbContext.Follows.Remove(follow);
                this.dbContext.SaveChanges();
                return true;
            }
            return false;
        }
    }

    public interface IFollowRepository
    {

        IQueryable<Follow> GetByUser(int userId);


        IQueryable<Follow> GetByProject(int ProjectId);


        Follow GetById(int id);

        Follow Get(int projectId, int userId);


        int Create(Follow follow);


        bool SaveChanges(Follow follow);


        bool IsFollowed(int projectId, int userId);

        bool Delete(Follow follow);
    }
}
