using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoG.Domain.Service;
using MoG.App_Start;
using Ninject;
using MoG.Domain.Models;
using MoG.Domain.Repository;
using System.IO;

namespace MoG.Test.Service
{
    [TestClass]
    public class ThumbnailServiceTest
    {
        private IThumbnailService service;


        [TestInitialize]
        public void MyTestInitialize()
        {
            var kernel = NinjectWebCommon.CreatePublicKernel();
            service = kernel.Get<IThumbnailService>();


        }



        [TestMethod]
        public void ThumbnailService_01_UploadTempThumbnailTest()
        {
           using (FileStream data = new FileStream("data/mabenz.png", FileMode.Open))
           {
               string result = this.service.StoreTemporaryProjectArtwork(1, data);

               Assert.IsTrue(!String.IsNullOrEmpty(result));
           }
           
        }

          [TestMethod]
        public void ThumbnailService_02_PromoteThumbnail()
        {
            bool result = this.service.PromoteTemporaryProjectArtwork(1).Result;
            Assert.IsTrue(result);
        }

     
    }
}
