using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoG.Domain.Service;
using MoG.App_Start;
using Ninject;
using MoG.Domain.Models;
using System.Linq;
using System.Collections.Generic;

namespace MoG.Test.Service
{
    [TestClass]
    public class MessageServiceTest
    {
        private IMessageService serviceMessage;
        private IUserService serviceUser;

        [TestInitialize]
        public void MyTestInitialize()
        {
            var kernel = NinjectWebCommon.CreatePublicKernel();
            serviceMessage = kernel.Get<IMessageService>();
            serviceUser = kernel.Get<IUserService>();
        }


        [TestMethod]
        public void MessageService_Send()
        {
            //Arrange
           List<UserProfile> dummyUsers = serviceUser.GetAll().ToList();
           Message test = new Message() { Body = "TEST", Title = "Title", Tag = "tag1#tag2" };
           test.DestinationIds = dummyUsers.Select(x => x.Id).ToList();

            //act
           var result = serviceMessage.Send(test);

           Assert.IsTrue(result.Id >0);

        }

         [TestMethod]
        public void MessageService_GetInbox()
        {
            //act
            var result = serviceMessage.GetInbox(1);

            //assert
            Assert.IsTrue(result.Count > 0);
        }

         [TestMethod]
        public void MessageService_GetSent()
        {
            //act
            var result = serviceMessage.GetSent(1).ToList();

            //assert
            Assert.IsTrue(result.Count > 0);
        }

       
    }
}
