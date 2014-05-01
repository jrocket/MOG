using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoG.App_Start;
using MoG.Domain.Service;
using Ninject;
using MoG.Domain.Models;

namespace Mog.Tests.Service
{
    [TestClass]
    public class UserServiceTest
    {
        IUserService service = null;
        [TestInitialize]
        public void MyTestInitialize()
        {
            var kernel = NinjectWebCommon.CreatePublicKernel();
            service = kernel.Get<IUserService>();
            //            serviceUser = kernel.Get<IUserService>();

        }


        [TestMethod]
        public void UserService_Search()
        {
            string searchQuery = "rocket";

           var  result = service.Search(searchQuery, 1, 10);

            Assert.IsTrue(result.Count > 0);
        }
    }
}
