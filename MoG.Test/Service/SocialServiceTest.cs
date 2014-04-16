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
    public class SocialServiceTest
    {
        private ISocialService service;


        [TestInitialize]
        public void MyTestInitialize()
        {
            var kernel = NinjectWebCommon.CreatePublicKernel();
            service = kernel.Get<ISocialService>();


        }

        [TestMethod]
        public void Social_GetFriends()
        {
            UserProfileInfo user = new UserProfileInfo();
            user.Id = 1;
            var friends = this.service.GetFriends(user);

            Assert.IsNotNull(friends);
            Assert.IsTrue(friends.Count > 0);
        }
    }
}
