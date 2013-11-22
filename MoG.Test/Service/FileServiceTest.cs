using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using MoG.Domain.Models;
using MoG.Domain.Service;
using MoG.App_Start;
using Ninject;

namespace MoG.Test.Service
{
    [TestClass]
    public class FileServiceTest
    {
        private IFileService serviceFile;

        [TestInitialize]
        public void MyTestInitialize()
        {
            var kernel = NinjectWebCommon.CreatePublicKernel();
            serviceFile = kernel.Get<IFileService>();
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
    }
}
