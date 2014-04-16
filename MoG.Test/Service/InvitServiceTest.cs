using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoG.Domain.Service;
using MoG.App_Start;
using Ninject;
using MoG.Domain.Models;
using MoG.Domain.Repository;
using System.IO;
using System.Linq;

namespace MoG.Test.Service
{
    [TestClass]
    public class InvitServiceTest
    {
        private IInvitService service;
        private IUserService serviceUser;

        [TestInitialize]
        public void MyTestInitialize()
        {
            var kernel = NinjectWebCommon.CreatePublicKernel();
            service = kernel.Get<IInvitService>();
            serviceUser = kernel.Get<IUserService>();

        }



        [TestMethod]
        public void InvitService_Invit()
        {
            Invit invit = new Invit();
            invit.ProjectId = 1;
            invit.UserId = 1;
            var user = serviceUser.GetAll().FirstOrDefault();
            int id = this.service.Invit(1, 1,"", user);

            Assert.IsTrue(id > 0);
        }
        [TestMethod]
        public void InvitService_GetInvits() 
        {
            //Arrange
            Invit invit = new Invit();
            invit.ProjectId = 1;
            invit.UserId = 1;
            var user = serviceUser.GetAll().FirstOrDefault();
            int id = this.service.Invit(1, 1,"", user);

            //Act
            var result = this.service.GetInvits(user.Id);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);

        }
        [TestMethod]
        public void InvitService_Accept()
        {
            //Arrange
            Invit invit = new Invit();
            invit.ProjectId = 1;
            invit.UserId = 1;
            var user = serviceUser.GetAll().FirstOrDefault();
            int id = this.service.Invit(1, 1,"", user);

            //Act
            var accepted = this.service.Accept(id);
            accepted = this.service.GetById(accepted.Id);

            //Assert
            Assert.IsNotNull(accepted);
            Assert.IsTrue(accepted.Status == InvitStatus.Accepted);
        }
        [TestMethod]
        public void InvitService_Reject()
        {  //Arrange
            Invit invit = new Invit();
            invit.ProjectId = 1;
            invit.UserId = 1;
            var user = serviceUser.GetAll().FirstOrDefault();
            int id = this.service.Invit(1, 1,"", user);

            //Act
            var rejected = this.service.Reject(id);
            rejected = this.service.GetById(rejected.Id);

            //Assert
            Assert.IsNotNull(rejected);
            Assert.IsTrue(rejected.Status == InvitStatus.Rejected);
        }
        [TestMethod]
        public void InvitService_GetById() 
        {
            //Arrange
            Invit invit = new Invit();
            invit.ProjectId = 1;
            invit.UserId = 1;
            var user = serviceUser.GetAll().FirstOrDefault();
            int id = this.service.Invit(1, 1,"", user);

            //Act

            var result = this.service.GetById(id);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id > 0);
        }



    }
}
