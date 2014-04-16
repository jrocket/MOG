using MoG.Domain.Models;
using MoG.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Service
{
    public class LikeService : ILikeService
    {
        private ILikeRepository repoLike = null;
        private IProjectService serviceProject = null;
        public LikeService(ILikeRepository repo, IProjectService projectService)
        {
            this.repoLike = repo;
            this.serviceProject = projectService;
        }


        public bool LikeIt(int projectId, int userId)
        {
            bool result = false;
            Like test = this.repoLike.Get(projectId, userId);
            if (test != null)
                return false;
            Like like = new Like() { ProjectId = projectId, UserId = userId };
            int createdId = this.repoLike.Create(like);
            if (createdId > 0)
            {
                this.serviceProject.IncreaseLike(projectId);
                result = true;
            }
            return result;
        }

        public int GetLikeCount(Project p)
        {
            return this.GetLikeCount(p.Id);

        }

        public int GetLikeCount(int projectId)
        {
            return this.repoLike.GetLikeCount(projectId);
        }


        public bool ResetLikeCount(int projectId)
        {
            bool result = false;
            if (this.repoLike.ResetLikeCount(projectId))
            {
                result = this.serviceProject.ResetLikeCount(projectId);
            } return result;

        }
    }

    public interface ILikeService
    {

        bool LikeIt(int projectId, int userId);

        int GetLikeCount(Project p);

        int GetLikeCount(int projectId);

        bool ResetLikeCount(int projectId);
    }
}
