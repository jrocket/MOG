using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoG.Domain.Service;
using Ninject;
using MoG.App_Start;
using MoG.Domain.Models;

namespace MoG.Test.Service
{
    [TestClass]
    public class SecurityServiceTest
    {
        private ISecurityService serviceSecurity;
//        private IUserService serviceUser;

        [TestInitialize]
        public void MyTestInitialize()
        {
            var kernel = NinjectWebCommon.CreatePublicKernel();
            serviceSecurity = kernel.Get<ISecurityService>();
  //          serviceUser = kernel.Get<IUserService>();
        }

        [TestMethod]
        public void SecurityService_EditProject()
        {
            //arrange
            UserProfileInfo u1 = new UserProfileInfo() { Login = "toto" };
            UserProfileInfo u2 = new UserProfileInfo() { Login = "tata" };
            Project p1 = new Project() { Creator = u1 };
            
            //act
            bool test1 = this.serviceSecurity.HasRight(SecureActivity.ProjectEdit, u2, p1);
            bool test2 = this.serviceSecurity.HasRight(SecureActivity.ProjectEdit, u1, p1);
            
            //assert
            Assert.IsFalse(test1,"u2 is not the creator -> does not has the right to edit");
            Assert.IsTrue(test2,"u1 is the creator -> has the right to edit");
        }
    }
}
