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
    public class LogServiceTest
    {
        private ILogService service;


        [TestInitialize]
        public void MyTestInitialize()
        {
            var kernel = NinjectWebCommon.CreatePublicKernel();
            service = kernel.Get<ILogService>();


        }



        [TestMethod]
        public void LogService_01_LogMessage()
        {
            int id = this.service.LogMessage("LogServiceTest::LogService_01_LogMessage", "log test...");


            Assert.IsTrue(id > 0);
        }

        [TestMethod]
        public void LogService_02_GetById()
        {
            string message = "log test...";
            int id = this.service.LogMessage("LogServiceTest::LogService_02_GetById", message);

            Log l = this.service.GetById(id);

            Assert.IsNotNull(l);
            Assert.IsTrue(l.Message == message);

        }
        [TestMethod]
        public void LogService_03_GetById()
        {
            string message = "log test...";
            int id = this.service.LogMessage("LogServiceTest::LogService_03_GetById", message);

            var logs = this.service.Get(0, 10);
            Assert.IsNotNull(logs);
            Assert.IsTrue(logs.Count > 0);

        }

    }
}
