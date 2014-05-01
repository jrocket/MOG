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
    public class DropBoxServiceTest
    {
        private IDropBoxService serviceDropBox = null;

        private IAuthCredentialRepository repoAuthCredential = null;



        [TestInitialize]
        public void MyTestInitialize()
        {
            var kernel = NinjectWebCommon.CreatePublicKernel();
            serviceDropBox = kernel.Get<IDropBoxService>();
            repoAuthCredential = kernel.Get<IAuthCredentialRepository>();

        }


        [TestMethod]
        public void DropBoxService_01_UploadFile()
        { 
            string file = @"Data/mabenz.mp3";
            var credentials = repoAuthCredential.GetByUserId(1).ToList();
            if (credentials.Count() > 0)
            {
                AuthCredential credential = credentials[0];
                MogFile uploadedFile = serviceDropBox.UploadFile(file, credential.Id);
                Assert.IsNotNull(uploadedFile);
                Assert.IsFalse(String.IsNullOrEmpty(uploadedFile.Path));
            }

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void DropBoxService_02_GetMediaUrl()
        {
            string path = "mabenz.mp3";

            var credentials = repoAuthCredential.GetByUserId(1).ToList();
            if (credentials.Count() > 0)
            {
                AuthCredential credential = credentials[0];
                string result = this.serviceDropBox.GetMedialUrl(path, credential);
                Assert.IsFalse(String.IsNullOrEmpty(result));
            }

            Assert.IsTrue(true);
        }
    }
}
