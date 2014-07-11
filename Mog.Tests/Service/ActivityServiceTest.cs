using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoG.Domain.Service;
using MoG.App_Start;
using Ninject;
using MoG.Domain.Models;
using MoG.Domain.Repository;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace MoG.Test.Service
{
    [TestClass]
    public class ActivityService
    {
        private IActivityService activityService;
        private IProjectService projectService;
        private INotificationRepository notificationRepository;
        private ICommentService commentService;
        [TestInitialize]
        public void MyTestInitialize()
        {
            var kernel = NinjectWebCommon.CreatePublicKernel();
            activityService = kernel.Get<IActivityService>();
            projectService = kernel.Get<IProjectService>();
            notificationRepository = kernel.Get<INotificationRepository>();
            commentService = kernel.Get<ICommentService>();

        }
        [TestMethod]
        public void Activity_NewProjectCreation()
        {
              //arrange
            int notificationCount = notificationRepository.GetCount();
           var projects = projectService.GetNew(1,10,false,false).ToList();
            if (projects.Count == 0)
            {
                throw new   Exception("please create a project before running this test");
            }

            //act
            activityService.LogProjectCreation(projects[0]);
              
            //assert
            
            Assert.IsTrue(notificationCount < notificationRepository.GetCount(),
@"there is no new notification created, please check if someone follows " + projects[0].Creator.DisplayName  );
        }

        [TestMethod]
        public void Activity_NewFileCreation()
        {
            //arrange
            int notificationCount = notificationRepository.GetCount();
            var projects = projectService.GetNew(1, 10, false, false).ToList();
            var project = projects.Where(p => p.Files.Count > 0).FirstOrDefault();
            if (project==null)
            {
                throw new Exception("please create a project with files before running this test");
            }

            //act

            activityService.LogFileCreation(project.Files.First());

            //assert

            Assert.IsTrue(notificationCount < notificationRepository.GetCount(),
@"there is no new notification created, please check if someone follows " + projects[0].Creator.DisplayName);
        }

        [TestMethod]
        public void Activity_NewCommentCreation()
        {
            //arrange
            int notificationCount = notificationRepository.GetCount();
            var projects = projectService.GetNew(1, 10, false, false).ToList();
            var project = projects.Where(p => p.Files.Count > 0).FirstOrDefault();
            if (project == null)
            {
                throw new Exception("please create a project with files and comments before running this test");
            }

            //act
            Comment c = new Comment()
            {
                Creator = project.Creator,
                File = project.Files.First(),
                ProjectId = project.Id
            };
            commentService.Create(c);
            //activityService.LogCommentCreation( c);

            //assert

            Assert.IsTrue(notificationCount < notificationRepository.GetCount(),
@"there is no new notification created, please check if someone follows " + projects[0].Creator.DisplayName);
        }


        [TestMethod]
        public void Activity_GetActivityByUser()
        {
            IList<Activity> activities = activityService.GetByUserId(1);

            Assert.IsTrue(activities.Count > 0);
        }

        [TestMethod]
        public void ActivityService_ReadNotifications()
        {

            int userId = 2;
            DateTime now = DateTime.Now;
            int unreadCount = activityService.GetUnreadCount(userId);
            if (unreadCount == 0)
            { return; }

            activityService.MarkNotificationsAsRead(userId);

            Assert.IsTrue(unreadCount > activityService.GetUnreadCount(userId));
        }
    }
}
