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

        private IUserService serviceUser { get; set; }

        public IProjectService ServiceProject { get; set; }

        private INotificationRepository repoNotification;

        public ActivityService(IActivityRepository _repo, IFollowService _followService
            , IInvitService _invitService
            , INotificationRepository _notificationRepo
            , IUserService _userService
                )
        {
            this.repoActivity = _repo;
            this.serviceFollow = _followService;
            this.serviceInvit = _invitService;
            this.repoNotification = _notificationRepo;
            this.serviceUser = _userService;
        }

        #region Actions

        public void LogProjectCreation(Project project)
        {
            Activity act = new Activity();
            act.ProjectId = project.Id;
            act.Who = project.Creator;
            act.When = DateTime.Now;
            act.Type = ActivityType.Create | ActivityType.Project;
            repoActivity.Create(act);

            notifyProjectCreation(project);

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

            notifyFileCreation(file);

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

            notifyCommentCreation(newComment);

        }

        #endregion Actions


        #region Notifications

        private void notifyFileCreation(ProjectFile file)
        {
            List<Notification> notifications = new List<Notification>();
            string message = String.Format("a new file was uploaded by {0} in {1} ",
                file.Creator.DisplayName
                , file.Project.Name);

            // get all the followers of the uploader
            var followers = serviceFollow.GetFollowerUsers(file.Creator.Id);

            foreach (var follower in followers)
            {
                //TODO : message translation + Url management
                Notification n = new Notification()
                {
                    Message = message,
                    UserId = follower.Id,
                    PictureUrl = file.Creator.PictureUrl,
                    Url = "/File/Display/" + file.Id
                };
                notifications.Add(n);
            }

            //get all the followers of the project
            var interrestedPeoples = serviceFollow.GetFollowsByProject(file.ProjectId);
            foreach (var follower in interrestedPeoples)
            {
                // do not add the notification if it was already added
                if (notifications.Where(n => n.UserId == follower.FollowerId).FirstOrDefault() == null)
                {
                    //TODO : message translation + Url management
                    Notification n = new Notification()
                    {
                        Message = message,
                        UserId = follower.Id,
                        PictureUrl = file.Creator.PictureUrl,
                        Url = "/File/Display/" + file.Id
                    };
                    notifications.Add(n);
                }
            }

            //Notify the project owner
            if (notifications.Where(n => n.UserId == file.Project.Creator.Id).FirstOrDefault() != null)
            {
                //TODO : message translation + Url management
                Notification n = new Notification()
                {
                    Message = message,
                    UserId = file.Project.Creator.Id,
                    PictureUrl = file.Creator.PictureUrl,
                    Url = "/File/Display/" + file.Id
                };
            }


            repoNotification.Create(notifications);
        }



        private void notifyProjectCreation(Project project)
        {

            var followers = serviceFollow.GetFollowerUsers(project.Creator.Id);
            List<Notification> notifications = new List<Notification>();
            foreach (var follower in followers)
            {
                //TODO : message translation + Url management
                Notification n = new Notification()
                {
                    Message = "a new project was created by " + project.Creator.DisplayName,
                    UserId = follower.Id,
                    PictureUrl = project.Creator.PictureUrl,
                    Url = "/Project/Detail/" + project.Id
                };
                notifications.Add(n);
            }
            repoNotification.Create(notifications);
        }

        private void notifyCommentCreation(Comment newComment)
        {
            List<Notification> notifications = new List<Notification>();


            string message = String.Format("a new comment was posted by {0} on {1} ",
                newComment.Creator.DisplayName
                , newComment.File.DisplayName);
            // get all the followers of the uploader
            var followers = serviceFollow.GetFollowerUsers(newComment.Creator.Id);

            foreach (var follower in followers)
            {
                //TODO : message translation + Url management
                Notification n = new Notification()
                {
                    Message = message,
                    UserId = follower.Id,
                    PictureUrl = newComment.Creator.PictureUrl,
                    Url = "/File/Display/" + newComment.FileId
                };
                notifications.Add(n);
            }

            //get all the followers of the project
            var interrestedPeoples = serviceFollow.GetFollowsByProject(newComment.File.ProjectId);
            foreach (var follower in interrestedPeoples)
            {
                // do not add the notification if it was already added
                if (!notifications.Any(n => n.UserId == follower.FollowerId))
                {
                    //TODO : message translation + Url management
                    Notification n = new Notification()
                    {
                        Message = message,
                        UserId = follower.FollowerId,
                        PictureUrl = newComment.Creator.PictureUrl,
                        Url = "/File/Display/" + newComment.FileId
                    };
                    notifications.Add(n);
                }
            }



            repoNotification.Create(notifications);
        }
        #endregion Notifications

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



        public List<Notification> GetNotificationByUserId(int userId, int count = -1)
        {
            IQueryable<Notification> data = this.repoNotification.GetByUserId(userId);
            if (count > 0)
            {
                data = data.Take(count);
            }
            return data.ToList();
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
        public List<Activity> GetActivitiesByUserId(int userId, int count = -1)
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

            var result = this.repoActivity.GetNotificationsByProjectIds(projectIds, userId);

            if (count > 0)
            {
                result = result.Take(count);
            }
            return result.ToList();
        }




        public List<Activity> GetLatest(Visibility visibility, int pageIndex, int pageSize)
        {
            if (ServiceProject == null)
            {
                throw new Exception("you must inject manually the serviceproject when you instanciate the ActivityService. Do something like activityServiceObject.serviceProject = ... ");

            }

            int skip = (pageIndex - 1) * pageSize;

            IQueryable<Activity> result = this.repoActivity.GetLatest(visibility)
                .Skip(skip)
                .Take(pageSize);


            return result.ToList();
        }


        public int GetUnreadCount(int userId)
        {
            IQueryable<Notification> unread = this.repoNotification.GetUnRead(userId);

            return unread.Count();
        }

        public void MarkNotificationsAsRead(int userId)
        {
            this.repoNotification.MarkAsRead(userId, DateTime.Now);
        }


        public List<SendNotificationVM> GetNotificationsToSend()
        {
            List<SendNotificationVM> result = new List<SendNotificationVM>();
            var notifs = this.repoNotification.GetUnSent().ToList();
            var userIds = notifs.Select(n => n.UserId).Distinct();
            var users = this.serviceUser.GetByIds(userIds);
            foreach (var user in users)
            {
                bool isNotificationToSend = false;
                switch (user.NotificationFrequency)
                {
                    case NotificationFrequency.Never:
                        break;
                    case NotificationFrequency.OnceADay:
                        isNotificationToSend = user.LastNotificationDate < DateTime.Now.AddDays(-1);
                        break;
                    case NotificationFrequency.OnceAnHour:
                        isNotificationToSend = user.LastNotificationDate < DateTime.Now.AddHours(-1);
                        break;
                    default:
                        break;
                }
                if (isNotificationToSend)
                {
                    result.Add(this.sendNotification(user, notifs));
                }

            }




            this.repoNotification.SetSent(notifs.Select(n => n.Id));

            foreach (var notif in result)
            {
                DateTime latestNotificationDate = notif.Notifications
             .OrderByDescending(n => n.CreatedOn)
             .First().CreatedOn;
                notif.User.LastNotificationDate = latestNotificationDate;
                this.serviceUser.SaveChanges(notif.User);
            }
            return result;
        }

        private SendNotificationVM sendNotification(UserProfileInfo user, List<Notification> notifs)
        {
            SendNotificationVM result = new SendNotificationVM();
            result.User = user;
            result.Notifications = notifs.Where(n => n.UserId == user.Id).ToList();
            return result;
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

        List<Notification> GetNotificationByUserId(int userId, int count = -1);

        List<Activity> GetActivitiesByUserId(int userId, int count = -1);
        List<Activity> GetLatest(Visibility visibility, int pageIndex, int pageSize);

        int GetUnreadCount(int userId);



        void MarkNotificationsAsRead(int userId);

        List<SendNotificationVM> GetNotificationsToSend();
    }
}
