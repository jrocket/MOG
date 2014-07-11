using MoG.Domain.Models;
using MoG.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Service
{
    public class FollowService : IFollowService
    {
        private IFollowRepository repo = null;

        public FollowService(IFollowRepository repo)
        {
            this.repo = repo;
        }


        public int Follow(int projectId, int userId)
        {
           
            Follow test = this.repo.GetFollowProject(projectId, userId);
            if (test != null)
            {
                return test.Id;
            }

            Follow follow = new Follow()
            {
                ProjectId = projectId
                , FollowerId = userId
            
            };
            int createdId = this.repo.Create(follow);

          
            return createdId;
        }

        public int FollowUser(int followedId, int followerId)
        {
            FollowUser test = this.repo.GetFollowedUser(followedId, followerId);
            if (test != null)
            {
                return test.Id;
            }

            FollowUser f = new FollowUser() { FollowedId = followedId, FollowerId = followerId };
            return this.repo.Create(f);
        }

      

        private bool SaveChanges(Follow follow)
        {
            return this.repo.SaveChanges(follow);
        }
      

        public bool Delete(int id)
        {
            Follow follow = this.GetById(id);

            return this.repo.DeleteFollowProject(follow); ;
        }


        public bool IsFollowed(Project project, UserProfileInfo user)
        {
            if (project == null || user == null)
                return false;
            return this.repo.IsFollowed(project.Id, user.Id);
        }


        public IList<Follow> GetFollowsByUser(int userId)
        {
            return this.repo.GetFollowedProjectByUser(userId)
                .Where(f => f.Project.VisibilityType == Visibility.Public)
                .ToList();
        }

        public IList<Follow> GetFollowsByProject(int projectId)
        {
            return this.repo.GetFollowerByProject(projectId).ToList();
        }

        public Follow GetById(int id)
        {
            return this.repo.GetFollowProjectById(id);
          
        }



        public Follow Get(int projectId, int userId)
        {
            return this.repo.GetFollowProject(projectId, userId);
        }


        public IEnumerable<int> GetFollowedProjectIds(int userId)
        {
            return this.repo.GetFollowedProjectByUser(userId).Select(f => f.ProjectId).ToList();
        }


        public IEnumerable<int> GetFollowedPublicProjectIds(int userId)
        {
            return this.repo.GetFollowedProjectByUser(userId)
                .Where(p => p.Project.VisibilityType != Visibility.Private)
                .Select(p => p.ProjectId)
                .ToList();
        }


        public    IList<FollowUser> GetFollowerUsers(int followedId)
        {
            return this.repo.GetFollowerUsers(followedId).ToList();
        }





    }

    public interface IFollowService
    {

        int Follow(int projectId, int userId);

        int FollowUser(int followedId, int followerId);
        IList<Follow> GetFollowsByUser(int userId);

        IList<FollowUser> GetFollowerUsers(int followedId);

        Follow Get(int projectId, int userId);
        IList<Follow> GetFollowsByProject(int projectId);
        Follow GetById(int id);


        bool Delete(int id);

        bool IsFollowed(Project project, UserProfileInfo user);

        IEnumerable<int> GetFollowedProjectIds(int userId);

        IEnumerable<int> GetFollowedPublicProjectIds(int userId);



    }
}
