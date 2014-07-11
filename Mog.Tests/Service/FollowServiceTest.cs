using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using MoG.Domain.Service;
using MoG.App_Start;
using MoG.Domain.Models;


namespace Mog.Tests.Service
{
    [TestClass]
    public class FollowServiceTest
    {
        private IFollowService service;
       
        [TestInitialize]
        public void MyTestInitialize()
        {
            var kernel = NinjectWebCommon.CreatePublicKernel();
            service = kernel.Get<IFollowService>();
          

        }

        [TestMethod]
        public void FollowService_Create()
        {
            var result = service.FollowUser(2, 1);
            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public void FollowService_GetFollowerUser()
        {
            var result = service.GetFollowerUsers(1);
            Assert.IsTrue(result.Count>0);
        }

       






    }
}
