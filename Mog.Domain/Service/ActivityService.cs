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

        public void LogFileCreation(MoGFile file)
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

    }
    public interface IActivityService
    {
        void LogProjectCreation(Project project);
        void LogFileCreation(MoGFile file);

        void LogCommentCreation(Comment newComment);

        IList<Activity> GetByProjectId(int projectId);

    }
}
