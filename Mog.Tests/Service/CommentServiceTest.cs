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
    public class CommentServiceTest
    {
        private ICommentService service;

        [TestInitialize]
        public void MyTestInitialize()
        {
            var kernel = NinjectWebCommon.CreatePublicKernel();
            service = kernel.Get<ICommentService>();
//            serviceUser = kernel.Get<IUserService>();

        }



       
        
        [TestMethod]
        public void Comment_GetCommentsByProject()
        {
            var comments = this.service.GetByProjectId(1);

            Assert.IsTrue(comments.Count > 0);
        }
    }
}
