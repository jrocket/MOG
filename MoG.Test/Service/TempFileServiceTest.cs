using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoG.Domain.Service;
using MoG.App_Start;
using Ninject;
using MoG.Domain.Models;

namespace MoG.Test.Service
{
    [TestClass]
    public class TempFileServiceTest
    {
        private ITempFileService serviceTempFile;
        private IUserService serviceUser;

        [TestInitialize]
        public void MyTestInitialize()
        {
            var kernel = NinjectWebCommon.CreatePublicKernel();
            serviceTempFile = kernel.Get<ITempFileService>();
            serviceUser = kernel.Get<IUserService>();
            
            serviceTempFile.DefaultSavePath = "/data";
        }



        [TestMethod]
        public void TempFileService_ProcessTest()
        {
            TempUploadedFile file = new TempUploadedFile();
            file.Description = "description";
            file.Name = "Name" + DateTime.Now.Ticks.ToString() ;
            file.Path = "Data/mabenz.mp3";
            file.ProjectId = 1;
            file.Tags = "tag1,tag2";

            var result = serviceTempFile.Process(file);

            Assert.IsTrue(result);
        }
    }
}
