using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using MoG.Domain.Models;
using MoG.Domain.Service;
using MoG.App_Start;
using Ninject;
using System.Linq;
using MoG.Domain.Repository;

namespace MoG.Test.Service
{
    [TestClass]
    public class FileServiceTest
    {
        private IFileService serviceFile;

        private IUserService serviceUser;

        private IProjectService serviceProject;

        private IAuthCredentialRepository repoCredential;

     
        
        [TestInitialize]
        public void MyTestInitialize()
        {
            var kernel = NinjectWebCommon.CreatePublicKernel();
            serviceFile = kernel.Get<IFileService>();
            serviceUser = kernel.Get<IUserService>();
            serviceProject = kernel.Get<IProjectService>();
            repoCredential = kernel.Get<IAuthCredentialRepository>();
        }



        [TestMethod]
        public void FileService_GetProjectFiles()
        {
            List<ProjectFile> files = serviceFile.GetProjectFile(1);

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
            var project = serviceProject.GetNew(1,10,false,false).ToList();
            var storageCredential = this.repoCredential.GetById(1);
            var user = serviceUser.GetAll().FirstOrDefault();
            ProjectFile f = new ProjectFile();
            f.Description = "Test + " + DateTime.Now.ToString();
            f.Tags = FileType.Drums.ToString();
            f.Likes = 42;
            f.DisplayName = "TEST FILE " + DateTime.Now.Ticks;
            f.InternalName = "TEST FILE INTERNAL " + DateTime.Now.Ticks;
            f.ProjectId = project[0].Id;
            f.AuthCredentialId = storageCredential.Id;



            int i = this.serviceFile.Create(f, user);
            var activities = this.serviceProject.GetProjectActivity(f.ProjectId);
            var fileActivities = activities.Where(a => (a.Type & ActivityType.File) == ActivityType.File).ToList();

            Assert.IsTrue(i > 0);
            Assert.IsTrue(f.Id > 0);
            Assert.IsTrue(fileActivities.Count > 0);

        }
    }
}
