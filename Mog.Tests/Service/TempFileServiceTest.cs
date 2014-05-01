using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoG.Domain.Service;
using MoG.App_Start;
using Ninject;
using MoG.Domain.Models;
using MoG.Domain.Repository;

namespace MoG.Test.Service
{
    [TestClass]
    public class TempFileServiceTest
    {
        private ITempFileService serviceTempFile;
        private IUserService serviceUser;
        private IAuthCredentialRepository repoAuthCredential;
     

        [TestInitialize]
        public void MyTestInitialize()
        {
            var kernel = NinjectWebCommon.CreatePublicKernel();
            serviceTempFile = kernel.Get<ITempFileService>();
            serviceUser = kernel.Get<IUserService>();
            repoAuthCredential = kernel.Get<IAuthCredentialRepository>();
            
          
        }



        [TestMethod]
        public void TempFileService_ProcessTest()
        {
            TempUploadedFile file = new TempUploadedFile();
            file.Description = "description";
            file.Name = "Test file Name"  ;
            file.Path = "Data/mabenz.mp3"; 
            file.ProjectId = 1;
            file.Tags = "tag1,tag2";
            file.AuthCredentialId = 1;
            file.Creator = serviceUser.GetCurrentUser();
            file.StorageCredential = repoAuthCredential.GetById(1);

            var result = serviceTempFile.Process(file);

            Assert.IsTrue(result);
        }

      
    }
}
