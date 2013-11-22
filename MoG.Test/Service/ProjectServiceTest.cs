﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoG.Domain.Service;
using MoG.App_Start;
using Ninject;
using System.Linq;
using MoG.Domain.Models;
using System.Collections.Generic;

namespace MoG.Test.Service
{
    [TestClass]
    public class ProjectServiceTest
    {
        private IProjectService serviceProject;
        private IUserService serviceUser;

        [TestInitialize]
        public void MyTestInitialize()
        {
            var kernel = NinjectWebCommon.CreatePublicKernel();
            serviceProject = kernel.Get<IProjectService>();
            serviceUser = kernel.Get<IUserService>();
        }


        [TestMethod]
        public void ProjectService_GetMyProject()
        {
            var collection = serviceProject.GetByUserLogin("jrocket");

            Assert.IsTrue(collection != null, "result must be different from null");

            Assert.IsTrue(collection.Count(p => true)>0, "collection size must be >0 - check your sample data");

            foreach (var item in collection)
            {
                Assert.IsTrue(item.Id > 0, "each project must have an id > 0");

            }
        }

        [TestMethod]
        public void ProjectService_GetNew()
        {
            int limit = 10;
            var collection = serviceProject.GetNew(limit);
            Assert.IsTrue(collection != null, "result must be different from null");

            Assert.IsTrue(collection.Count(p => true) > 0, "collection size must be >0 - check your sample data");

            int count = 0;
            DateTime testDate = DateTime.MaxValue;
            foreach (var item in collection)
            {
                count++;
                Assert.IsTrue(item.CreatedOn < testDate,"project must be ordered in descending created date");
                testDate = item.CreatedOn;
                Assert.IsTrue(item.Id > 0, "each project must have an id > 0");

            }
            Assert.IsTrue(count <= limit, "must no retrieve more result than asked");
        }

        [TestMethod]
        public void ProjectService_GetPopular()
        {
            //Arrange
            int limit = 10;

            //Act
            var collection = serviceProject.GetPopular(limit);
            
            //Assert
            Assert.IsTrue(collection != null, "result must be different from null");

            Assert.IsTrue(collection.Count(p => true) > 0, "collection size must be >0 - check your sample data");

            int count = 0;
            int  testLike = Int32.MaxValue;
            foreach (var item in collection)
            {
                count++;
                Assert.IsTrue(item.Likes < testLike, "project must be ordered in descending like order");
                testLike = item.Likes;
                Assert.IsTrue(item.Id > 0, "each project must have an id > 0");

            }
            Assert.IsTrue(count <= limit, "must no retrieve more result than asked");
        }

        [TestMethod]
        public void ProjectService_GetRandom()
        {
            //Arrange
            int limit = 10;

            //Act
            var collection = serviceProject.GetRandom(limit);

            //Assert
            Assert.IsTrue(collection != null, "result must be different from null");

            Assert.IsTrue(collection.Count(p => true) > 0, "collection size must be >0 - check your sample data");

            int count = 0;
            foreach (var item in collection)
            {
                count++;
                Assert.IsTrue(item.Id > 0, "each project must have an id > 0");

            }
            Assert.IsTrue(count <= limit, "must no retrieve more result than asked");
        }
         [TestMethod]
        public void ProjectService_GetById()
        {
            int id = 1;

            var project = serviceProject.GetById(id);

            Assert.IsNotNull(project);
            Assert.IsTrue(project.Id == 1);
        }
        [TestMethod]
        public void ProjectService_Create()
         {
             Project p = new Project();
             p.LicenceType = MoG.Licence.CCBY;
             p.Description = "Description";
             p.Likes = 0;
             p.Name = DateTime.Now.ToLongTimeString() + " - TEST";
             p.Tags = "ROCK POP TEST";
             p.VisibilityType = MoG.Visibility.Private;

            var user = serviceUser.GetCurrentUser();

            int i = serviceProject.Create(p, user);

            Assert.IsTrue(i > 0);
            Assert.IsTrue(p.Id > 0);
         }


        [TestMethod]
        public void ProjectService_GetProjectActivity()
        {
            var activites = serviceProject.GetProjectActivity(1);

            Assert.IsNotNull(activites);
           
            Assert.IsTrue(activites.Count>0);

        }

    
        [TestMethod]
        public void ProjectService_GetProjectFiles2()
        {
            var project = serviceProject.GetById(1);

            ICollection<MoGFile> files = project.Files;

            Assert.IsNotNull(files);
            Assert.IsTrue(files.Count > 0);

        }
        [TestMethod]
        public void ProjectService_GetFileStatus()
        {
            var project = serviceProject.GetById(1);
            var statuses = serviceProject.GetFileStatuses(project);

            Assert.IsNotNull(statuses);
            Assert.IsTrue(statuses.Count > 0);
        }
        [TestMethod]
        public void ProjectService_GetFileAuthors()
        {
            var project = serviceProject.GetById(1);
            var authors = serviceProject.GetFileAuthors(project);

            Assert.IsNotNull(authors);
            Assert.IsTrue(authors.Count > 0);
        }
        [TestMethod]
        public void ProjectService_GetFileTypes()
        {
            var project = serviceProject.GetById(1);
            var types = serviceProject.GetFileTypes(project);

            Assert.IsNotNull(types);
            Assert.IsTrue(types.Count > 0);
        }

        [TestMethod]
        public void ProjectService_GetFilesFilteredByType()
        {
            var project = serviceProject.GetById(1);
            var files = serviceProject.GetFilteredFiles(project, "", "", FileType.Bass.ToString());

            Assert.IsNotNull(files);
            Assert.IsNotNull(project);
            Assert.IsNotNull(project.Files);

            Assert.IsTrue(files.Count < project.Files.Count);
        }

        [TestMethod]
        public void ProjectService_GetFilesFilteredByStatus()
        {
            var project = serviceProject.GetById(1);
            var files = serviceProject.GetFilteredFiles(project, "", FileStatus.Accepted.ToString(),"");

            Assert.IsNotNull(files);
            Assert.IsNotNull(project);
            Assert.IsNotNull(project.Files);

            Assert.IsTrue(files.Count < project.Files.Count);
        }
    }
}