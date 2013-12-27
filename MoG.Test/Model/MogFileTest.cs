using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoG.Domain.Models;

namespace MoG.Test.Model
{
    [TestClass]
    public class MogFileTest
    {
        [TestMethod]
        public void MogFile_Metadata()
        {
            Mp3Metadata data = new Mp3Metadata();
            data.Duration = "4:07";

            MoGFile file = new MoGFile();
            file.SetMetadata(data);

            var retrievedMetadata  = file.GetMetadata() as Mp3Metadata;

            Assert.IsNotNull(retrievedMetadata);
            Assert.IsFalse(String.IsNullOrEmpty(retrievedMetadata.Duration));
        }



    }
}
