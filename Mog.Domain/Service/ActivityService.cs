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
      
        private IFollowService serviceFollow;

        private IInvitService serviceInvit;
        public IProjectService ServiceProject {get;set;}
      
        public ActivityService(IActivityRepository _repo,IFollowService _followService, IInvitService _invitService)
        {
            this.repoActivity = _repo;
            this.serviceFollow = _followService;
            this.serviceInvit = _invitService;
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

            result = result.Where(a => (a.ProjectId != null ? a.Project.VisibilityType == Visibility.Public : true));
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


        /// <summary>
        /// Notifications for activities
        ///  - on project I created
        ///  - on projects I follow (except if the project is private)
        ///  - on my friends (except on private projects)
        ///  - on projects i'm invited to
        ///  
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<Activity> GetNotificationForUserId(int userId, int count = -1)
        {
            if (ServiceProject == null)
            {
                throw new Exception("you must inject manually the serviceproject when you instanciate the ActivityService. Do something like activityServiceObject.serviceProject = ... ");

            }
            //find my projects
            List<int> projectIds = ServiceProject.GetProjectIds(userId);

            //find the projects I follow which are not private
            projectIds.AddRange(serviceFollow.GetFollowedPublicProjectIds(userId));

            //find the project I got invited (and I accepted)
            projectIds.AddRange(serviceInvit.GetAcceptedInvitsProjectIds(userId));


            //find the people I follow
            //Todo : find the people a user follows

            var result = this.repoActivity.GetNotificationsByProjectIds( projectIds, userId);

            if (count > 0)
            {
                result = result.Take(count);
            }
            return result.ToList();
        }


    }
    public interface IActivityService
    {
        IProjectService ServiceProject { get; set; }

        void LogProjectCreation(Project project);
        void LogFileCreation(ProjectFile file);

        void LogCommentCreation(Comment newComment);

        IList<Activity> GetByProjectId(int projectId);


        List<Activity> GetByUserId(int id, int count = -1);

        bool DeleteByCommentId(int id);

        List<Activity> GetNotificationForUserId(int userId, int count = -1);
    }
}
