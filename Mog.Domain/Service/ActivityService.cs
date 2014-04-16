using MoG.Domain.Models;
using MoG.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Service
{
    public class ActivityService : IActivityService
    {

        private IActivityRepository repoActivity;

        public ActivityService(IActivityRepository _repo)
        {
            this.repoActivity = _repo;
        }

        public void LogProjectCreation(Project project)
        {
            Activity act = new Activity();
            act.ProjectId = project.Id;
            act.Who = project.Creator;
            act.When = DateTime.Now;
            act.Type = ActivityType.Create | ActivityType.Project;
            repoActivity.Create(act);
        }

        public void LogFileCreation(ProjectFile file)
        {
            Activity act = new Activity();
            act.FileId = file.Id;
            act.ProjectId = file.ProjectId;
            act.Who = file.Creator;
            act.When = DateTime.Now;
            act.Type = ActivityType.Create | ActivityType.File;
            repoActivity.Create(act);
        }




        public void LogCommentCreation(Comment newComment)
        {
            Activity act = new Activity();
            act.Who = newComment.Creator;
            act.CommentId = newComment.Id;
            act.ProjectId = newComment.ProjectId;
            act.FileId = newComment.FileId;
            act.When = DateTime.Now;
            act.Type = ActivityType.Create | ActivityType.Comment;
            repoActivity.Create(act);
        }


        public IList<Activity> GetByProjectId(int projectId)
        {
            return this.repoActivity.GetByProjectId(projectId).ToList();
        }



        public List<Activity> GetByUserId(int id, int count = -1)
        {
            var result = this.repoActivity.GetByUserId(id);
            if (count > 0)
            {
                result = result.Take(count);
            }
            return result.ToList();
        }


        public bool DeleteByCommentId(int id)
        {
            var activity = this.repoActivity.GetByCommentId(id);
            return this.repoActivity.Delete(activity);
        }
    }
    public interface IActivityService
    {
        void LogProjectCreation(Project project);
        void LogFileCreation(ProjectFile file);

        void LogCommentCreation(Comment newComment);

        IList<Activity> GetByProjectId(int projectId);


        List<Activity> GetByUserId(int id, int count = -1);

        bool DeleteByCommentId(int id);
    }
}
