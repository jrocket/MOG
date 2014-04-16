using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoG.Domain.Service;
using Ninject;
using MoG.App_Start;
using MoG.Domain.Models;
using System.Collections.Generic;

namespace MoG.Test.Service
{
    [TestClass]
    public class DownloadCartServiceTest
    {
        private IDownloadCartService serviceDownload = null;
        private IUserService serviceUser;


        [TestInitialize]
        public void MyTestInitialize()
        {
            var kernel = NinjectWebCommon.CreatePublicKernel();
            serviceDownload = kernel.Get<IDownloadCartService>();
            serviceUser = kernel.Get<IUserService>();
           
        }

        [TestMethod]
        public void DownloadCart_01_AddToCart()
        {
            int fileId = 1;
            UserProfileInfo user = serviceUser.GetCurrentUser();

            int result = serviceDownload.AddToCart(fileId, user);
          

            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public void DownloadCart_02_GetCart()
        { 
            UserProfileInfo user = serviceUser.GetCurrentUser();

            IList<DownloadCartItem> downloadCart = serviceDownload.GetByUserId(user.Id);

            Assert.IsTrue(downloadCart.Count > 0);
            DownloadCartItem first = downloadCart[0];
            Assert.IsNotNull(first);
            Assert.IsNotNull(first.File);
            Assert.IsFalse(String.IsNullOrEmpty(first.File.PublicUrl));
        }

        [TestMethod]
        public void DownloadCart_03_DeleteFromCart()
        {
            int fileId = 1;
            UserProfileInfo user = serviceUser.GetCurrentUser();
            int insertedId = serviceDownload.AddToCart(fileId, user);

            bool result = serviceDownload.Delete(insertedId,user);


            Assert.IsTrue(result);
          
        }

        [TestMethod]
        public void DownloadCart_04_ClearCart()
        {
          
            UserProfileInfo user = serviceUser.GetCurrentUser();
            int fileId = 1;
           
            int insertedId = serviceDownload.AddToCart(fileId, user);
            bool result = serviceDownload.ClearCart(user.Id);

          


            Assert.IsTrue(result);
            IList<DownloadCartItem> downloadCart = serviceDownload.GetByUserId(user.Id);
            Assert.IsTrue(downloadCart.Count == 0);


        } 
    }
}
