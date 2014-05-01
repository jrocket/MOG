using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoG.Domain.Service;
using MoG.App_Start;
using Ninject;
using MoG.Domain.Models;
using MoG.Domain.Repository;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace MoG.Test.Service
{
    [TestClass]
    public class ActivityService
    {
        private IActivityService service;

        [TestInitialize]
        public void MyTestInitialize()
        {
            var kernel = NinjectWebCommon.CreatePublicKernel();
            service = kernel.Get<IActivityService>();

        }





        [TestMethod]
        public void Comment_GetActivityByUser()
        {
            IList<Activity> activities = service.GetByUserId(1);

            Assert.IsTrue(activities.Count > 0);
        }
    }
}
