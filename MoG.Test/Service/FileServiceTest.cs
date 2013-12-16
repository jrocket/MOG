using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using MoG.Domain.Models;
using MoG.Domain.Service;
using MoG.App_Start;
using Ninject;
using System.Linq;

namespace MoG.Test.Service
{
    [TestClass]
    public class FileServiceTest
    {
        private IFileService serviceFile;

        private IUserService serviceUser;

        private IProjectService serviceProject;
        
        [TestInitialize]
        public void MyTestInitialize()
        {
            var kernel = NinjectWebCommon.CreatePublicKernel();
            serviceFile = kernel.Get<IFileService>();
            serviceUser = kernel.Get<IUserService>();
            serviceProject = kernel.Get<IProjectService>();
        }



        [TestMethod]
        public void FileService_GetProjectFiles()
        {
            List<MoGFile> files = serviceFile.GetProjectFile(1);

            Assert.IsNotNull(files);
            Assert.IsTrue(files.Count > 0);

        }
        [TestMethod]
        public void FileService_GetFile()
        {
            int fileId = 1;
            var file = serviceFile.GetById(fileId);

            Assert.IsNotNull(file);
            Assert.IsTrue(file.Id == fileId);
            Assert.IsNotNull(file.Project);
        }

        [TestMethod]
        public void FileService_GetComments()
        {
            int fileId = 1;
            var messages = serviceFile.GetFileComments(fileId);


            Assert.IsNotNull(messages);

            foreach (var message in messages)
            {
                Assert.IsTrue(message.FileId == fileId);
            }


        }
        [TestMethod]
        public void FileService_Create()
        {
            var project = serviceProject.GetNew(10).ToList();
            var user = serviceUser.GetCurrentUser();
            MoGFile f = new MoGFile();
            f.Description = "Test + " + DateTime.Now.ToString();
            f.FileType = FileType.Drums;
            f.Likes = 42;
            f.Name = "TEST FILE " + DateTime.Now.Ticks;
            f.ProjectId = project[0].Id;
            f.Tags = "TAGS TEST";



            int i = this.serviceFile.Create(f, user);
            var activities = this.serviceProject.GetProjectActivity(f.ProjectId);
            var fileActivities = activities.Where(a => (a.Type & ActivityType.File) == ActivityType.File).ToList();

            Assert.IsTrue(i > 0);
            Assert.IsTrue(f.Id > 0);
            Assert.IsTrue(fileActivities.Count > 0);

        }
    }
}
