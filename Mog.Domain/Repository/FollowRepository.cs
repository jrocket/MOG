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

        public int Create(FollowUser follow)
        {
            follow.CreatedOn = DateTime.Now;
            this.dbContext.FollowUsers.Add(follow);
            this.dbContext.SaveChanges();
            return follow.Id;
        }



        public bool SaveChanges(Follow follow)
        {
            this.dbContext.Entry(follow).State = System.Data.Entity.EntityState.Modified;
            int result = this.dbContext.SaveChanges();
            return result > 0;
        }

        public bool SaveChanges(FollowUser follow)
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

        public IQueryable<Follow> GetFollowedProjectByUser(int userId)
        {
            this.dbContext.Configuration.ProxyCreationEnabled = false;

            return this.dbContext.Follows
                .Include(f => f.Follower)
                .Include(f => f.Project)
             .Where(i => i.FollowerId == userId);
        }

        public IQueryable<Follow> GetFollowerByProject(int ProjectId)
        {
            this.dbContext.Configuration.ProxyCreationEnabled = false;
            return this.dbContext.Follows
                .Include(f => f.Follower)
              .Where(i => i.ProjectId == ProjectId);
        }

        public Follow GetFollowProjectById(int id)
        {
            return this.dbContext.Follows.Find(id);
        }

        public Follow GetFollowProject(int projectId, int userId)
        {
            return this.dbContext.Follows
                .Where(i => i.FollowerId == userId && i.ProjectId == projectId).FirstOrDefault();
        }

        public FollowUser GetFollowedUser(int followedId, int followerId)
        {
            return this.dbContext.FollowUsers
               .Where(i => i.FollowerId == followerId && i.FollowedId == followedId).FirstOrDefault();
        }



        public bool DeleteFollowProject(Follow follow)
        {

            if (follow != null)
            {
                this.dbContext.Follows.Remove(follow);
                this.dbContext.SaveChanges();
                return true;
            }
            return false;
        }


        public IQueryable<FollowUser> GetFollowedUsers(int userId)
        {
            return this.dbContext.FollowUsers.Where(f => f.FollowerId == userId);
        }
        public IQueryable<FollowUser> GetFollowerUsers(int followedId)
        {
            return this.dbContext.FollowUsers.Where(f => f.FollowedId == followedId);
        }
       

        







    }

    public interface IFollowRepository
    {

        IQueryable<Follow> GetFollowedProjectByUser(int userId);


        IQueryable<Follow> GetFollowerByProject(int ProjectId);

        IQueryable<FollowUser> GetFollowedUsers(int userId);

        Follow GetFollowProjectById(int id);

        Follow GetFollowProject(int projectId, int userId);

        FollowUser GetFollowedUser(int followedId, int followerId);

        int Create(Follow follow);

        int Create(FollowUser follow);

        bool SaveChanges(Follow follow);

        bool SaveChanges(FollowUser follow);


        bool IsFollowed(int projectId, int userId);

   
        bool DeleteFollowProject(Follow follow);

        IQueryable<FollowUser> GetFollowerUsers(int followedId);



    }
}
