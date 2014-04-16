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
           
            Follow test = this.repo.Get(projectId, userId);
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

      

        private bool SaveChanges(Follow follow)
        {
            return this.repo.SaveChanges(follow);
        }
      

        public bool Delete(int id)
        {
            Follow follow = this.GetById(id);

            return this.repo.Delete(follow); ;
        }


        public bool IsFollowed(Project project, UserProfileInfo user)
        {
            return this.repo.IsFollowed(project.Id, user.Id);
        }


        public IList<Follow> GetFollowsByUser(int userId)
        {
            return this.repo.GetByUser(userId).ToList();
        }

        public IList<Follow> GetFollowsByProject(int projectId)
        {
            return this.repo.GetByProject(projectId).ToList();
        }

        public Follow GetById(int id)
        {
            return this.repo.GetById(id);
          
        }



        public Follow Get(int projectId, int userId)
        {
            return this.repo.Get(projectId, userId);
        }
    }

    public interface IFollowService
    {

        int Follow(int projectId, int userId);
        IList<Follow> GetFollowsByUser(int userId);

        Follow Get(int projectId, int userId);
        IList<Follow> GetFollowsByProject(int projectId);
        Follow GetById(int id);


        bool Delete(int id);

        bool IsFollowed(Project project, UserProfileInfo user);
    }
}
