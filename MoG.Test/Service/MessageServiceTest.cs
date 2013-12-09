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
        public void MessageService_TestArchiveInbox()
        {
            //Arrange
            List<UserProfile> dummyUsers = serviceUser.GetAll().ToList();
            Message test = new Message() { Body = "TEST MessageService_TestArchiveInbox", Title = "Title", Tag = "tag1#tag2" };
            var destinationIds = dummyUsers.Select(x => x.Id).ToList();
            serviceMessage.Send(test, destinationIds,null);
            var currentUser = serviceUser.GetCurrentUser();
            var inbox = serviceMessage.GetFolder(currentUser.Id, MogConstants.MESSAGE_INBOX).ToList();
            var firstMessage = inbox[0];
            int inboxCount = inbox.Count;

            //Act
            serviceMessage.Archive(firstMessage.BoxId, currentUser);

            //Assert
            inbox = serviceMessage.GetFolder(currentUser.Id, MogConstants.MESSAGE_INBOX).ToList();
            Assert.IsTrue(inboxCount != inbox.Count);


        }
        [TestMethod]
        public void MessageService_TestArchiveOutbox()
        {
            //Arrange
            List<UserProfile> dummyUsers = serviceUser.GetAll().ToList();
            Message test = new Message() { Body = "TEST MessageService_TestArchiveOutbox", Title = "Title", Tag = "tag1#tag2" };
            var destinationIds = dummyUsers.Select(x => x.Id).ToList();
            serviceMessage.Send(test, destinationIds,null);
            var currentUser = serviceUser.GetCurrentUser();
            var outbox = serviceMessage.GetFolder(currentUser.Id, MogConstants.MESSAGE_OUTBOX).ToList();
            var firstMessage = outbox[0];
            int inboxCount = outbox.Count;

            //Act
            serviceMessage.Archive(firstMessage.BoxId, currentUser);
              var archives = serviceMessage.GetFolder(currentUser.Id, MogConstants.MESSAGE_ARCHIVE);

            //Assert
            outbox = serviceMessage.GetFolder(currentUser.Id, MogConstants.MESSAGE_OUTBOX).ToList();
            Assert.IsTrue(inboxCount != outbox.Count);
            Assert.IsTrue(archives.Count()>0);
           

          
          
        }


        [TestMethod]
        public void MessageService_Send()
        {
            //Arrange
            List<UserProfile> dummyUsers = serviceUser.GetAll().ToList();
            Message test = new Message() { Body = "TEST", Title = "Title", Tag = "tag1#tag2" };
            var destinationIds = dummyUsers.Select(x => x.Id).ToList();

            //act
            var result = serviceMessage.Send(test, destinationIds,null);

            Assert.IsTrue(result.Id > 0);

        }

        [TestMethod]
        public void MessageService_GetInbox()
        {
            //act
            var result = serviceMessage.GetFolder(1, MogConstants.MESSAGE_INBOX).ToList();

            //assert

            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void MessageService_GetSent()
        {
            //act
            var result = serviceMessage.GetFolder(1, MogConstants.MESSAGE_OUTBOX).ToList();

            //assert
            Assert.IsTrue(result.Count > 0);
        }


    }
}
